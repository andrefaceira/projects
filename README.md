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
    
    
    
Install prometheus

    # install
    helm install prometheus prometheus-community/kube-prometheus-stack --version 45.10.1

    ## namespace is default, should be other
    ## install via yaml with values
    # https://marketplace.digitalocean.com/apps/kubernetes-monitoring-stack


