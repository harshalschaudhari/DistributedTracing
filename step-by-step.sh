# Build docker image for UserService and CalculatorService

# Step 1: Build a project using 
# dotnet build

# Go to respective path /DistributedTracing
docker build -t user-service-api:v1.0 -f ./UserRequestService/Dockerfile .

# Go to respective path /DistributedTracing
docker build -t calculator-service-api:v1.0 -f ./CalculatorService/Dockerfile .

# Docker run 
docker run -d -p 8084:8080 --name user-container user-service-api:v1.0
docker run -d -p 8085:8080 --name calculator-container calculator-service-api:v1.0

# Docker Compose

docker-compose up

docker-compose down

# Get into pod
docker exec -u root -it podName /bin/sh

# Install curl
 ```console
apt-get -y update; apt-get -y install curl

curl --version


 ```
 # Install vi
 ```console
apt-get update;  apt-get install vim
 ```

# Request from UserRequestService pod
 ```console
curl -X GET "http://localhost:8080/api/UserRequest/add?num1=10&num2=11"
 ```

# Request call from UserRequestService pod
 ```console
curl -X GET "http://calculator-service-api:8080/api/Calculator/add?num1=12&num2=13"
 ```

 # Run jaeger for local testing
  ```console
 docker run -d --name jaeger   -e COLLECTOR_ZIPKIN_HTTP_PORT=9411   -p 5775:5775/udp   -p 6831:6831/udp   -p 6832:6832/udp   -p 5778:5778   -p 16686:16686   -p 14268:14268   -p 9411:9411   jaegertracing/all-in-one:1.6
  ```
