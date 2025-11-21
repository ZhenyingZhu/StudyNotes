# Kubernetes

## K8s vs. Docker

Kubernetes can use Docker as the container runtime (though today, containerd or CRI-O are more common).

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

Isolation level

- <https://docs.docker.com/guides/getting-started/develop-with-containers/>

- can use vhd
- <https://gist.github.com/jjmartres/177c68d3f44a4f8a82d9d0bc42a97637>
- <https://docs.docker.com/get-started/overview/>
- <https://docs.docker.com/guides/docker-concepts/the-basics/what-is-a-container/>
- <https://hub.docker.com/_/microsoft-windows-base-os-images>

Docker node image: <https://hub.docker.com/_/node/>

Command

```powershell
# build
docker build --build-arg=DOTNET_EnableWriteXorExecute=0 -f client/Dockerfile -t k8s-client .

# run
$WORKSPACE=<path>
docker run -ti --platform=linux/amd64 --privileged --network=host --user root --volume $WORKSPACE/k8s-infrastructure/ --volume $WORKSPACE/src/client/.azure:/root/.azure/ --volume $WORKSPACE/src/client/.kube/:/root/.kube --volume $WORKSPACE/src/client/.bashrc_local:/root/.bashrc_local --volume $WORKSPACE/src/client/.bash_history:/root/.bash_history --workdir /k8s-infrastructure/src/client k8s-client
```

<https://stackoverflow.com/questions/26028971/docker-container-ssl-certificates>

The registry needs to use Azure account to login.

### Log

<https://techcommunity.microsoft.com/t5/containers/windows-containers-log-monitor-opensource-release/ba-p/973947>

### ServiceMonitor

<https://github.com/microsoft/IIS.ServiceMonitor>

### File lock issue

<https://stackoverflow.com/questions/53836103/failed-to-create-endpoint-on-network-nat-hnscall-failed-in-win32-the-process-c>

### Copy files out

<https://stackoverflow.com/questions/22049212/copying-files-from-docker-container-to-host>

### Update hosts

Only work for linux:

```powershell
docker run --add-host host1:ip1 --add-host host1:ip2 --isolation=process -d -p 4445:4445 -t buildlabel -p bootstrapper.json
```

In Windows, follow <https://stackoverflow.com/questions/53268105/how-to-use-the-arg-instruction-of-dockerfile-for-windows-image>

In the Dockerfile

```powershell
ARG ip="1.1.1.1"
ARG host="host"

RUN ECHO %ip% %host% >> "C:\Windows\System32\drivers\etc\hosts"
RUN ipconfig /flushdns
```

In the command, run

```powershell
docker build --build-arg ip=$ip --build-arg host=$host -t ${tag} $TargetPath
```
