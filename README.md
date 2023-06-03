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

    kubectl create namespace argocd
    kubectl apply -n argocd -f https://raw.githubusercontent.com/argoproj/argo-cd/stable/manifests/install.yaml

    kubectl port-forward svc/argocd-server -n argocd 8080:443

    argocd admin initial-password -n argocd
    argocd login localhost:8080 --port-forward-namespace argocd
    argocd account update-password --port-forward-namespace argocd

TODO: change password and create app: https://cloud.redhat.com/blog/how-to-use-argocd-deployments-with-github-tokens
TODO: make git repository private