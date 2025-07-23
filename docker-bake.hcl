
variable "CONTRAST_AGENT_VERSION" {
    description = "Target version of the Contrast Security agent to use"
    default = "latest"
}

group "default" {
    targets = ["runtime", "runtime-with-contrast", "tests"]
}

target "docker-metadata-action" {}

target "build" {
    inherits = ["docker-metadata-action"]
    target = "build"
    context = "."
    dockerfile = "Dockerfile"
}

target "runtime" {
    inherits = ["docker-metadata-action"]
    context = "."
    dockerfile = "Dockerfile"
    target = "runtime"
}

target "runtime-with-contrast" {
    inherits = ["docker-metadata-action"]
    context = "."
    dockerfile = "Dockerfile"
    target = "runtime-with-contrast"
    args = {
        CONTRAST_AGENT_VERSION = CONTRAST_AGENT_VERSION
    }
}

target "tests" {
    inherits = ["docker-metadata-action"]
    context = "./tests"
    dockerfile = "Dockerfile"
}
