# docker build - Go to respective path /DistributedTracing
docker build --platform linux/arm64 -t user-service-api-arm:v1.0 -f ./UserRequestService/Dockerfile .

docker tag user-service-api-arm:v1.0 harshalschaudhari/user-service-api-arm:v1.0

docker build -t user-service-api:v2.0 -f ./UserRequestService/Dockerfile .

docker build -t user-service-api:v3.0 -f ./UserRequestService/Dockerfile .

docker build --platform linux/arm64 -t user-service-api-arm:v3.0 -f ./UserRequestService/Dockerfile .

# Step - Map local docker image to DockerHub artifactory
docker tag user-service-api:v1.0 harshalschaudhari/user-service-api:v1.0

docker tag user-service-api:v2.0 harshalschaudhari/user-service-api:v2.0

docker tag user-service-api:v3.0 harshalschaudhari/user-service-api:v3.0

docker tag user-service-api-arm:v3.0 harshalschaudhari/user-service-api-arm:v3.0

# Step - Push local docker image to GitHub artifactory
# login to Container repository
docker push harshalschaudhari/user-service-api:v1.0

docker push harshalschaudhari/user-service-api-arm:v1.0

docker push harshalschaudhari/user-service-api:v2.0

docker push harshalschaudhari/user-service-api:v3.0

docker push harshalschaudhari/user-service-api-arm:v3.0