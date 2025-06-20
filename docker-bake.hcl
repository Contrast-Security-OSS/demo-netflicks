
variable "CONTRAST_AGENT_VERSION" {
    description = "Target version of the Contrast Security agent to use"
    default = "latest"
}

group "default" {
    targets = ["runtime", "runtime-with-contrast", "tests"]
}

group "multiarch" {
    targets = ["runtime-multiarch", "runtime-with-contrast-multiarch", "tests-multiarch"]
}

target "runtime" {
    context = "."
    dockerfile = "Dockerfile"
    target = "runtime"
    no-cache = true
    tags = [
        "contrastsecuritydemo/netflicks:latest"
    ]
}

target "runtime-multiarch" {
    inherits = ["runtime"]
    platforms = [
        "linux/amd64",
        "linux/arm64"
    ]
}

target "runtime-with-contrast" {
    context = "."
    dockerfile = "Dockerfile"
    target = "runtime-with-contrast"
    args = {
        CONTRAST_AGENT_VERSION = CONTRAST_AGENT_VERSION
    }
    tags = ["contrastsecuritydemo/netflicks:latest-contrast"]
}

target "runtime-with-contrast-multiarch" {
    inherits = ["runtime-with-contrast"]
    platforms = [
        "linux/amd64",
        "linux/arm64"
    ]
}

target "tests" {
    context = "./tests"
    dockerfile = "Dockerfile"
    tags = [
        "e2e-tests/netflicks:latest"
    ]
}

target "tests-multiarch" {
    inherits = ["tests"]
    platforms = [
        "linux/amd64",
        "linux/arm64"
    ]
}
