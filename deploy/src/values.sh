### These labels affect how your app is viewed in Tanzu Observability and K8S
export K8S_NAMESPACE=tacocat-homelab
export K8S_APPLICATION=tacocat-homelab
export K8S_CLUSTER=tacocat-homelab
export K8S_LOCATION=homelab-roswell
export K8S_ENVIRONMENT=dev

# export K8S_NAMESPACE=tacocat-463-tas
# export K8S_APPLICATION=tacocat-haas-464-tas
# export K8S_CLUSTER=tas-463
# export K8S_LOCATION=haas-463-tas
# export K8S_ENVIRONMENT=dev

### Use default to use prebuilt containers 
### Otherise add your registry here (with trailing slash)
### leave empty for local containers
export K8S_REPOSITORY=public.ecr.aws/tanzu_observability_demo_app/to-demo/
#export K8S_REPOSITORY=""
#export K8S_REPOSITORY=192.168.1.8/demo-app
#export K8S_REPOSITORY=public.ecr.aws/z4m0n1r4/to-cf-demo/

### Update with your Tanzu Observability by Wavefront info
export WAVEFRONT_BASE64_TOKEN=Y2UzYmFiZGYtMGZhMi00NmUwLThjZjctMDQxZDM0NzQ2MDY1
export WAVEFRONT_URL=http://longboard.wavefront.com/api

### Define for Tanzu Application Service Deploye
### otherwise leave empty
export TASDOMAIN=""
#export TASDOMAIN=-service.apps.internal

### Use default to deploy a proxy to the K8S namespace
### Edit for For Docker or Tanzu Application Service deploy
export WF_PROXY_HOST=${K8S_NAMESPACE}-wavefront-proxy  
# export WF_PROXY_HOST="192.168.1.6"
#export WF_PROXY_HOST="10.213.165.11"


