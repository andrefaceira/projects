# Projects

## Getting started

### Infrastructure

Run terraform

    terraform init
    terraform plan
    terraform apply
    
Get k8s_id from the output

    doctl kubernetes cluster kubeconfig save {k8s_id}

Install ArgoCD

    # install
    kubectl create namespace argocd
    kubectl apply -n argocd -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml

    # run
    kubectl port-forward svc/argocd-server -n argocd 8080:443

    # change default password
    argocd admin initial-password -n argocd
    argocd login localhost:8080 --port-forward-namespace argocd
    argocd account update-password --port-forward-namespace argocd

    # create app
    ## TODO: make git repository private
    argocd app create faceira --repo https://github.com/andrefaceira/projects --revision feature/infrastructure --path infrastructure/k8s --dest-namespace default --dest-server https://kubernetes.default.svc --directory-recurse
    
    argocd app sync faceira
    argocd app sync prometheus
    
    
    
Install prometheus

    https://artifacthub.io/packages/helm/prometheus-community/kube-prometheus-stack

    # install
    cd infrastructure/k8s/
    kubectl create namespace prometheus
    helm install prometheus prometheus-community/kube-prometheus-stack -n prometheus -f ./prometheus-values.yaml --version 46.5.0

    # verify
    kubectl get pods -n prometheus
    kubectl get services -n prometheus

    # run
    kubectl port-forward svc/prometheus-kube-prometheus-prometheus 9090:9090 -n prometheus
    kubectl port-forward svc/prometheus-grafana 3000:80 -n prometheus


Install metrics server?

    kubectl apply -f https://github.com/kubernetes-sigs/metrics-server/releases/latest/download/components.yaml



Install Nats

    https://artifacthub.io/packages/helm/nats/nats

    # install
    helm repo add nats https://nats-io.github.io/k8s/helm/charts/
    helm repo update

    kubectl create namespace nats
    helm install nats nats/nats -n nats -f ./nats-values.yaml --version 0.19.14

    # verify
    kubectl exec -n nats -it deployment/nats-box -- /bin/sh -l



Install open search

    # install 
    helm repo add opensearch https://opensearch-project.github.io/helm-charts/
    helm repo update
    helm search repo opensearch

    kubectl create namespace opensearch
    helm install opensearch opensearch/opensearch -n opensearch -f ./opensearch-values.yaml --version 2.12.2
    helm install opensearch-dashboards opensearch/opensearch-dashboards -n opensearch -f ./opensearch-dashboards-values.yaml --version 2.10.0

