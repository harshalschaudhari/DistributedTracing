# docker build - Go to respective path /DistributedTracing
docker build --platform linux/arm64 -t calculator-service-api-arm:v1.0 -f ./CalculatorService/Dockerfile .

# Step - Map local docker image to DockerHub artifactory
docker tag calculator-service-api:v1.0 harshalschaudhari/calculator-service-api:v1.0

docker tag calculator-service-api-arm:v1.0 harshalschaudhari/calculator-service-api-arm:v1.0


# Step - Push local docker image to GitHub artifactory
# login to Container repository
docker push harshalschaudhari/calculator-service-api:v1.0
docker push harshalschaudhari/calculator-service-api-arm:v1.0


http://localhost:32769/swagger/index.html

http://localhost:32769/api/Calculator/add?num1=10&num2=12

http://localhost:32769/WeatherForecast


# jaeger for local testing
docker run -d   -p 6831:6831/udp  -p 6832:6832/udp  -p 14268:14268    -p 14250:14250    -p 16686:16686    -p 5778:5778   --name jaeger jaegertracing/all-in-one:1.22