# Kubernetes

## K8s vs. Docker

<https://k21academy.com/docker-kubernetes/kubernetes-vs-docker/#:~:text=Docker%20provides%20basic%20networking%20capabilities,policies%20out%20of%20the%20box.>

## Docker

<https://docs.docker.com/get-started/overview/>

- daemon `dockerd`: listen for API requests
- client `docker`: sending commands to `dockerd`
- registries: stores images
- images: read-only template. Build image: create a Dockerfile to define steps, seperated by layers to make it lightweight.
- container: runnable instance of an image
- `docker pull ubuntu`: 1st layer
- `docker container create`: allocates a R/W file system. Final layer. Also creates a network interface
- `docker run -i -t ubuntu /bin/bash`: starts the container, execute bash with iteractive mode and attached to the terminal
- `exit`: stop the container

- <https://docs.docker.com/guides/getting-started/develop-with-containers/>

- can use vhd
- <https://gist.github.com/jjmartres/177c68d3f44a4f8a82d9d0bc42a97637>
- <https://docs.docker.com/get-started/overview/>
- <https://docs.docker.com/guides/docker-concepts/the-basics/what-is-a-container/>
- <https://hub.docker.com/_/microsoft-windows-base-os-images>

Command

```powershell
# build
docker build --build-arg=DOTNET_EnableWriteXorExecute=0 -f client/Dockerfile -t k8s-client .

# run
$WORKSPACE=<path>
docker run -ti --platform=linux/amd64 --privileged --network=host --user root --volume $WORKSPACE/k8s-infrastructure/ --volume $WORKSPACE/src/client/.azure:/root/.azure/ --volume $WORKSPACE/src/client/.kube/:/root/.kube --volume $WORKSPACE/src/client/.bashrc_local:/root/.bashrc_local --volume $WORKSPACE/src/client/.bash_history:/root/.bash_history --workdir /k8s-infrastructure/src/client k8s-client
```

<https://stackoverflow.com/questions/26028971/docker-container-ssl-certificates>
