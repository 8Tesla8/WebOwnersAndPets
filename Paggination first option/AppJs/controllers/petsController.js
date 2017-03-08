ownerApp.controller('petController',
    function($scope, $http, $location) {


        var ownerId = $location.hash(); //get getParam from url 

        var getRequest = {
            method: 'GET',
            url: 'http://localhost:11319/api/pet/?owner=' + ownerId
        };

        var update = function(response) {
            $scope.ownerName = response.data[0].OwnerName;

            if (response.data[0].Id != 0) {
                $scope.pets = response.data;
                $scope.petsCount = response.data.length;
            } else {
                $scope.petsCount = 0;
                $scope.pets = null;
            }
        };


        var serverIsNotResonse = "server is not avaliabel, status code: ";
        var dataIsNotSended = "Your data is not sended, status code: ";


        //delete 
        $scope.deletePet = function(event) {

            var deleteRequest = {
                method: 'DELETE',
                url: 'http://localhost:11319/api/pet/' + event.target.id
            }

            $http(deleteRequest).then(function() {
                //update
                $http(getRequest).then(function(response) {
                    update(response);
                }, function(response) {
                    alert(serverIsNotResonse + response.status);
                });
                //
            }, function() {
                alert(dataIsNotSended + response.status);
            });
        }
        //delete

        //post
        $scope.addPet = function(name, petForm) {
            if (petForm.$valid) {

                var postRequest = {
                    method: 'POST',
                    url: 'http://localhost:11319/api/pet',
                    data: {
                        OwnerId: ownerId,
                        Name: name
                    }
                }

                $http(postRequest).then(function() {
                    //update
                    $http(getRequest).then(function(response) {
                        update(response);
                        $scope.name = null;
                    }, function(response) {
                        alert(serverIsNotResonse + response.status);
                    });
                    //
                }, function() {
                    alert(dataIsNotSended + response.status);
                });
            }
        }
        //post

        //get 
        $http(getRequest).then(function(response) {
            update(response);
        }, function(response) {
            alert(serverIsNotResonse + response.status);
        });
        //

        //pagination 
        $scope.currentPage = 0;
        $scope.itemsPerPage = 3;

        $scope.firstPage = function () {
            return $scope.currentPage == 0;
        }
        $scope.lastPage = function() {
            if ($scope.pets && $scope.pets.length) {
                lastPageNum = Math.ceil($scope.pets.length / $scope.itemsPerPage - 1);
                return $scope.currentPage == lastPageNum;
            }
        }
        $scope.numberOfPages = function () {
            if ($scope.pets && $scope.pets.length) {
                return Math.ceil($scope.pets.length / $scope.itemsPerPage);
            }
        }
        $scope.startingItem = function () {
            return $scope.currentPage * $scope.itemsPerPage;
        }
        $scope.pageBack = function () {
            $scope.currentPage = $scope.currentPage - 1;
        }
        $scope.pageForward = function () {
            $scope.currentPage = $scope.currentPage + 1;
        }
        //
    }
);