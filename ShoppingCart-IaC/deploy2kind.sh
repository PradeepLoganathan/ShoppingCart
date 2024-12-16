#create cluster
kind create cluster --config ShoppingCart-IaC/kind-config.yaml

#setup namespace
kubectl apply -f ShoppingCart-IaC/namespace.yaml

#setup sqlserver
kubectl apply -f sqlserver-deployment.yaml
kubectl apply -f sqlserver-service.yaml

#setup product-api
kubectl apply -f productapi-deployment.yaml

kubectl apply -f productapi-service.yaml

kubectl apply -f productapi-ingress.yaml

kubectl port-forward service/productapi-service 8080:80