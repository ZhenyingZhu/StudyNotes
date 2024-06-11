# Kubernetes

## K8s vs. Docker

<https://k21academy.com/docker-kubernetes/kubernetes-vs-docker/#:~:text=Docker%20provides%20basic%20networking%20capabilities,policies%20out%20of%20the%20box.>

## Docker

<https://docs.docker.com/get-started/overview/>

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
