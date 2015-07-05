App.controller('FormGroupController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal) {
        $scope.groups = [];

        $scope.fetchGroups = function () {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'FormGroup/Get', {
                params: {
                    portal: $scope.isDesignPortal() == true ? 1 : 2
                }
            })
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

        /*=-=-=-=-=-=-=-= Start Add =-=-=-=-=-=-=-=*/
        $scope.newGroup = {
            Portal: $scope.isDesignPortal() == true ? 1 : 2,
            Title: '',
            Column: 1,
            Priority: 0
        };

        $scope.add = function () {
            if ($scope.addFormGroupForm.$invalid) return;

            $scope.addLoading = true;
            $http.post(baseUri + 'FormGroup/Add',
            {
                model: $scope.newGroup
            }).
            success(function (data, status, headers, config) {
                $scope.addLoading = false;
                $scope.newGroup = {
                    Portal: $scope.isDesignPortal() == true ? 1 : 2,
                    Title: '',
                    Column: 1,
                    Priority: 0
                };
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
        /*=-=-=-=-=-=-=-= End Add =-=-=-=-=-=-=-=*/

        $scope.show = function (index) {
            $scope.fetchLoading = true;
            $http.post(baseUri + 'FormGroup/Show',
            {
                id: $scope.groups[index].Id
            }).
            success(function (data, status, headers, config) {
                $scope.fetchLoading = false;
                if (data == "True") {
                    $scope.groups[index].IsShow = true;
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
        };

        $scope.hide = function (index) {
            $scope.fetchLoading = true;
            $http.post(baseUri + 'FormGroup/Hide',
            {
                id: $scope.groups[index].Id
            }).
            success(function (data, status, headers, config) {
                $scope.fetchLoading = false;
                if (data == "True") {
                    $scope.groups[index].IsShow = false;
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
                    $http.post(baseUri + 'FormGroup/Remove',
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
                $scope.groups[index] = result;
            }, function () {
            });
        };

        var EditCtrl = function ($scope, $http, $modalInstance, group) {

            $scope.group = group;

            $scope.edit = function () {
                if ($scope.editFormGroupForm.$invalid) return;

                $scope.editLoading = true;
                $http.post(baseUri + 'FormGroup/Edit',
                {
                    model: $scope.group
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
        };

        //init
        $scope.fetchGroups();
    }]);