# Kubernetes w/Contrast .NET Core agent Example

This guide provides a working example of setting up and configuring the Contrast .NET Core agent within a kubernetes environment.   This guide assumes you have basic working knowledge of git, docker and kubernetes.

## Prerequisites

Setup a local kubernetes environment if you do not already have one available to you.  Below are a few of the available options.

- Install Docker & kubernetes, follow instructions on [https://docs.docker.com/docker-for-mac/install](https://docs.docker.com/docker-for-mac/install/)/
- Install and start Minikube, follow instructions on https://kubernetes.io/docs/tasks/tools/install-minikube/

Clone the following repo that will be used for this tutorial:

```shell
git clone https://github.com/Contrast-Security-OSS/demo-netflicks.git
```

## Building and setting up the .NET Core application's image w/Contrast

1.  In your terminal, navigate to the cloned repo's folder and run the following command to build the docker image.

```shell
docker build -f Dockerfile.contrast . -t netflicks:1.0
```

* * *

Inspect the `Dockerfile`, you should note a few sections where the Contrast agent is added:

Line 13-20 utilizes a build stage to fetch in the latest Contrast .NET Core agent.

```dockerfile
# Add contrast sensor package
FROM ubuntu:bionic AS contrast
RUN set -xe \
    && apt-get update \
    && apt-get install -y curl unzip \
    && curl -v -L https://www.nuget.org/api/v2/package/Contrast.SensorsNetCore -o /tmp/contrast.sensorsnetcore.nupkg \
    && unzip /tmp/contrast.sensorsnetcore.nupkg -d /tmp/contrast \
    && rm /tmp/contrast.sensorsnetcore.nupkg
```

Line 30 copies in the resulting directory to the final image.

```dockerfile
COPY --from=contrast /tmp/contrast /opt/contrast
```

For more details on adding the Contrast agent to your application/image. [See our docker guide on the subject](https://support.contrastsecurity.com/hc/en-us/articles/360052815632--NET-Core-agent-with-Docker).

* * *

2.  Tag and push your image to a local or public repo.

```shell
docker tag netflicks:contrast example/netflicks:contrast
``````shell
docker push example/netflicks:contrast
```

Now this image can be used in the kubernetes deployment.

## Setting up the Kubernetes environment

2.  Download the `contrast_security.yaml` and create a secret

YAML file should look like the following for our Node.js agent:

```yaml
api: 
  url: http(s)://<dns>/Contrast
  api_key: <apiKey>
  service_key: <serviceKey>
  user_name: agent_<hashcode>@<domain>
```

Create a secret using `kubectl`

```shell
kubectl create secret generic contrast-security --from-file=./contrast_security.yaml
```

> This secret can be used by all .Net Core agents. So it is preferable to keep this generic and make all app level configuration changes with environmental variables.

3.  Create the applications deployment file and add the contrast_security secret. This will mount the file under `/etc/contrast/`

```yaml
---
apiVersion: v1
kind: Service
metadata:
  name: netflicks-db-service
spec:
  selector:
    app: netflicks-db
  ports:
    - protocol: TCP
      port: 1433
      targetPort: 1433
  type: ClusterIP
---
apiVersion: v1
kind: Service
metadata:
  name: netflicks-controller-svc
spec:
  type: LoadBalancer
  selector:
    app: netflicks
  ports:
    - name: "netflicks"
      port: 8080
      targetPort: 80
---
apiVersion: v1
kind: ReplicationController
metadata:
  name: netflicks-controller
spec:
  replicas: 1
  template:
    metadata:
      labels:
        app: netflicks
    spec:
      containers:
      - name: netflicks-db
        image: mcr.microsoft.com/mssql/server:2019-latest
        ports:
        - containerPort: 1433
        env:
        - name: ACCEPT_EULA
          value: "Y"
        - name: SA_PASSWORD
          value: "reallyStrongPwd123"

      - name: netflicks
        image: example/netflicks:contrast
        ports:
        - containerPort: 80
        envFrom:
        - configMapRef:
            name: contrast-config 
        volumeMounts:
        - name: contrast-security
          readOnly: false
          mountPath: "/etc/contrast/"
        env: 
        - name: ConnectionStrings__DotNetFlicksConnection
          value: "Server=tcp:localhost,1433;Initial Catalog=DotNetFlicksDb;Persist Security Info=False;User ID=sa;Password=reallyStrongPwd123;MultipleActiveResultSets=False;"
      volumes:
      - name: contrast-security
        secret:
          secretName: contrast-security
```

4.  Add application level configurations to setup logging, pointer to the `contrast_security.yaml` and any desired name/environment updates. A full list of configurations options are provided in [our documentation here](https://docs.contrastsecurity.com/en/environment-variables.html)

```yaml
env:
        - name: CONTRAST__APPLICATION__NAME
          value: "netflicks-k8s"
        - name: CONTRAST__SERVER__NAME
          value: "EKS-Core-Pod"
        - name: CONTRAST__SERVER__ENVIRONMENT
          value: "QA"
        - name: CONTRAST_CONFIG_PATH
          value: "/etc/contrast/contrast_security.yaml"
        - name: AGENT__LOGGER__STDOUT
          value: "true"
        - name: AGENT__LOGGER__LEVEL
          value: "INFO"
        - name: CONTRAST__AGENT__SERVICE__LOGGER__PATH
          value: "/proc/1/fd/1"
        - name: CONTRAST__AGENT__SERVICE__LOGGER__LEVEL
          value: "INFO"
        - name: CORECLR_PROFILER_PATH_64
          value: "/opt/contrast/contentFiles/any/netstandard2.0/contrast/runtimes/linux-x64/native/ContrastProfiler.so"
        - name: CORECLR_PROFILER
          value: "{8B2CE134-0948-48CA-A4B2-80DDAD9F5791}"
        - name: CORECLR_ENABLE_PROFILING
          value: "1"
        - name: CONTRAST_CORECLR_LOGS_DIRECTORY
          value: "/opt/contrast"
```

**Optionally:** these could also be defined via configmaps

Create a file called `contrast.properties` with the same environmental variables defined.

```shell
CONTRAST__APPLICATION__NAME=netflicks-k8s
CONTRAST__SERVER__NAME=EKS-Core-Pod
CONTRAST__SERVER__ENVIRONMENT=qa
CONTRAST_CONFIG_PATH=/etc/contrast/contrast_security.yaml
AGENT__LOGGER__STDOUT=true
AGENT__LOGGER__LEVEL=INFO
CORECLR_PROFILER_PATH_64=/opt/contrast/contentFiles/any/netstandard2.0/contrast/runtimes/linux-x64/native/ContrastProfiler.so
CORECLR_PROFILER={8B2CE134-0948-48CA-A4B2-80DDAD9F5791}
CORECLR_ENABLE_PROFILING=1
CONTRAST_CORECLR_LOGS_DIRECTORY=/opt/contrast
```

Create the configmap

```shell
kubectl create configmap contrast-config --from-env-file=contrast.properties
```

Update the deployment file to reference the configmap.

```yaml
spec:
      containers:
      - name: netflicks
        image: example/netflicks-k8s:contrast
        ports: 
          - containerPort: 3000
        envFrom:
          - configMapRef:
              name: contrast-config
```

5.  Apply the deployment file

```shell
kubectl apply -f netflicks_deployment.yaml
```

6.  Check on the deployed application in kubernetes and access the site at External-IP/port 8080

```shell
kubectl get all
```

The sources for this example can be found on our github repo: [demo-netflicks](https://github.com/Contrast-Security-OSS/demo-netflicks)
