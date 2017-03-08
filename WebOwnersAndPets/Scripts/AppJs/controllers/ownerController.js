ownerApp.controller('ownerController',
    function ($scope, $http) {

        var getRequest = {
            method: 'GET',
            url: 'http://localhost:11319/api/owner'
        };

        var update = function (response) {
            $scope.owners = response.data;
            $scope.ownersCount = response.data.length;
        };


        var serverIsNotResonse = "server is not avaliabel, status code: ";
        var dataIsNotSended = "Your data is not sended, status code: ";

        //delete 
        $scope.deleteOwner = function (event) {

            var deleteRequest = {
                method: 'DELETE',
                url: 'http://localhost:11319/api/owner/' + event.target.id
            }

            $http(deleteRequest).then(function () {
                //update
                $http(getRequest).then(function (response) {
                    update(response);
                }, function (response) {
                    alert(serverIsNotResonse + response.status);
                });
                //
            }, function () {
                alert(dataIsNotSended + response.status);
            });
        }
        //delete

        //post
        $scope.addOwner = function (name, ownerForm) {
            if (ownerForm.$valid) {

                var postRequest = {
                    method: 'POST',
                    url: 'http://localhost:11319/api/owner',
                    data: { Name: name }
                }

                $http(postRequest).then(function() {                   
                    //update
                    $http(getRequest).then(function (response) {
                        $scope.name = null;
                        update(response);
                    }, function (response) {
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
        $http(getRequest).then(function (response) {
            update(response);
        }, function (response) {
            alert(serverIsNotResonse + response.status);
        });
        //

        $scope.pageSize = 3;

    }
)