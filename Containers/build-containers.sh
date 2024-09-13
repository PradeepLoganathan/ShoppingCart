docker build -t pradeepl/productapi:latest -f ProductApi/Dockerfile .

docker build -t pradeepl/shoppingcartapi:latest -f ShoppingCartApi/Dockerfile .

docker build -t pradeepl/shoppingcartapp:latest -f ShoppingCartApp/Dockerfile .

docker build -t pradeepl/shoppingcartweb:latest -f ShoppingCartWeb/Dockerfile .

docker login

docker push pradeepl/productapi:latest

docker push pradeepl/shoppingcartapi:latest

docker push pradeepl/shoppingcartapp:latest

docker push pradeepl/shoppingcartweb:latest
