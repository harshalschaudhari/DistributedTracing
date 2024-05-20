docker-compose up -d

docker-compose up
docker-compose down

# Ghost from Browser
http://localhost:8080/

#hello echo from Browser
http://localhost:8081/

# Test Jaeger from Browser
http://localhost:16686/search

#Test from nginx pod
apt update
apt install curl

curl http://jaeger:16686/search
curl "http://calculator-service-api:8080/api/Calculator/add?num1=3&num2=49"
curl "http://user-service-api:8080/api/UserRequest/add?num1=5&num2=24"

# Test below url from Browser
http://localhost:8082/api/UserRequest/add?num1=5&num2=24

http://localhost:8083/api/Calculator/add?num1=3&num2=49

#Jaeger
http://localhost:16686/search

# Reference 
https://docs.nginx.com/nginx/admin-guide/dynamic-modules/opentelemetry/

