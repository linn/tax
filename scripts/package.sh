#!/bin/bash
set -ev

# build dotnet application
dotnet publish ./src/Service.Host/ -c release
dotnet publish ./src/Messaging.Host/ -c release

# determine which branch this change is from
if [ "${TRAVIS_PULL_REQUEST}" = "false" ]; then
  GIT_BRANCH=$TRAVIS_BRANCH
else
  GIT_BRANCH=$TRAVIS_PULL_REQUEST_BRANCH
fi

# create a docker
docker build --no-cache -t linn/tax:$TRAVIS_BUILD_NUMBER --build-arg gitBranch=$GIT_BRANCH ./src/Service.Host/
#docker build --no-cache -t linn/tax-messaging:$TRAVIS_BUILD_NUMBER --build-arg gitBranch=$GIT_BRANCH ./src/Messaging.Host/

# push to dockerhub
docker login -u $DOCKER_HUB_USERNAME -p $DOCKER_HUB_PASSWORD 
docker push linn/tax:$TRAVIS_BUILD_NUMBER
#docker push linn/tax-messaging:$TRAVIS_BUILD_NUMBER
