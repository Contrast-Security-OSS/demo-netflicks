#!/bin/bash

echo "Please log in using your Docker Hub credentials to update the container image"
docker login
docker tag netflicks:1.0 contrastsecuritydemo/netflicks:1.0
docker push contrastsecuritydemo/netflicks:1.0