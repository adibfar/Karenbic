App.controller('CustomerGroupController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal) {
        $scope.groups = [];

        $scope.fetchGroups = function () {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'CustomerGroup/Get')
            .success(function (data, status, headers, config) {
                $scope.groups = data.Data;
                $scope.fetchLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchLoading = false;
            });
        };

        $scope.add = function () {
            if ($scope.addCustomerGroupForm.$invalid) return;

            $scope.addLoading = true;
            $http.post(baseUri + 'CustomerGroup/Add',
            {
                model: {
                    Title: $scope.newTitle
                }
            }).
            success(function (data, status, headers, config) {
                $scope.addLoading = false;
                $scope.newTitle = '';
                $scope.groups.push(data);
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                }
                $scope.addLoading = false;
            });
        };

        $scope.remove = function (index) {
            ngDialog.open({
                template: 'removeDialog.html',
                showClose: false,
                controller: ['$scope', 'ngDialog', function ($scope, ngDialog) {
                    $scope.close = function () {
                        $scope.closeThisDialog(0);
                    };
                    $scope.confirm = function () {
                        $scope.closeThisDialog(1);
                    };
                }],
                preCloseCallback: function (value) {
                    if (value != 1) return true;

                    $scope.fetchLoading = true;
                    $http.post(baseUri + 'CustomerGroup/Remove',
                    {
                        id: $scope.groups[index].Id
                    }).
                    success(function (data, status, headers, config) {
                        $scope.fetchLoading = false;
                        if (data == "True") {
                            $scope.groups.splice(index, 1);
                        }
                        else {
                            toaster.pop('error', "امکان حذف اطلاعات وجود ندارد");
                        }
                    }).error(function (data, status, headers, config) {
                        if (status == 403) {
                            window.location = "/Account/Login";
                        }
                        else {
                            toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                        }
                        $scope.fetchLoading = false;
                    });
                    return true;
                }
            });
        };

        $scope.showEditModal = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/EditModalContent.html',
                controller: EditCtrl,
                size: 'xs',
                resolve: {
                    group: function () {
                        return _.clone($scope.groups[index]);
                    }
                }
            });

            modalInstance.result.then(function (result) {
                $scope.groups[index].Title = result.Title;
            }, function () {
            });
        };

        var EditCtrl = ['$scope', '$http', '$modalInstance', 'group', function ($scope, $http, $modalInstance, group) {

            $scope.group = group;

            $scope.edit = function () {
                if ($scope.editCustomerGroupForm.$invalid) return;

                $scope.editLoading = true;
                $http.post(baseUri + 'CustomerGroup/Edit',
                {
                    model: {
                        Id: $scope.group.Id,
                        Title: $scope.group.Title
                    }
                }).
                success(function (data, status, headers, config) {
                    $scope.editLoading = false;
                    $modalInstance.close(data);
                }).error(function (data, status, headers, config) {
                    if (status == 403) {
                        window.location = "/Account/Login";
                    }
                    else {
                        toaster.pop('error', "خطایی رخ داده دوباره امتحان کنید");
                    }
                    $scope.editLoading = false;
                });
            };

            $scope.close = function () {
                $modalInstance.dismiss('cancel');
            };
        }];

        //init
        $scope.fetchGroups();
    }]);