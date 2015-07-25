App.controller('PortfolioCategoriesController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$upload',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal, $upload) {
        $scope.types = [];
        $scope.categories = [];

        $scope.fetchTypes = function () {
            $scope.fetchTypesLoading = true;

            $http.get(baseUri + 'PortfolioType/Get')
            .success(function (data, status, headers, config) {
                $scope.types = data.Data;
                $scope.newCategory.Type = $scope.types[0];
                $scope.fetchTypesLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchTypesLoading = false;
            });
        };

        $scope.fetchCategories = function () {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'PortfolioCategory/Get')
            .success(function (data, status, headers, config) {
                $scope.categories = data.Data;
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
        $scope.newCategory = {
            Title: '',
            Type: null,
            Priority: 0
        };

        $scope.add = function () {
            if ($scope.addCategoryForm.$invalid) return;

            $scope.addLoading = true;

            $http.post(baseUri + 'PortfolioCategory/Add',
                {
                    title: $scope.newCategory.Title,
                    typeId: $scope.newCategory.Type.Id,
                    priority: $scope.newCategory.Priority
                }).
                success(function (data, status, headers, config) {
                    $scope.addLoading = false;
                    $scope.newCategory = {
                        Title: '',
                        Type: null,
                        Priority: 0
                    };
                    $scope.categories.push(data);
                    $scope.addLoading = false;
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
                    $http.post(baseUri + 'PortfolioCategory/Remove',
                    {
                        id: $scope.categories[index].Id
                    }).
                    success(function (data, status, headers, config) {
                        $scope.fetchLoading = false;
                        if (data == "True") {
                            $scope.categories.splice(index, 1);
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
                size: 'sm',
                resolve: {
                    types: function () {
                        return _.clone($scope.types);
                    },
                    category: function () {
                        var category = {
                            Id: $scope.categories[index].Id,
                            Title: $scope.categories[index].Title,
                            Priority: $scope.categories[index].Priority,
                            Type: {
                                Id: $scope.categories[index].Type.Id,
                                Title: $scope.categories[index].Type.Title
                            }
                        };
                        return category;
                    }
                }
            });

            modalInstance.result.then(function (result) {
                $scope.categories[index].Title = result.Title;
                $scope.categories[index].Type.Id = result.Type.Id;
                $scope.categories[index].Type.Title = result.Type.Title;
                $scope.categories[index].Priority = result.Priority;
            }, function () {
            });
        };

        var EditCtrl = ['$scope', '$http', '$modalInstance', 'types', 'category', function ($scope, $http, $modalInstance, types, category) {

            $scope.types = types;
            $scope.category = category;
            $scope.newPictureFile = null;

            var typeIndex = _.findIndex($scope.types, function (item) {
                return item.Id = $scope.category.Type.Id;
            });
            $scope.category.Type = $scope.types[typeIndex];

            $scope.edit = function () {
                $http.post(baseUri + 'PortfolioCategory/Edit',
                {
                    id: $scope.category.Id,
                    title: $scope.category.Title,
                    typeId: $scope.category.Type.Id,
                    priority: $scope.category.Priority
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
        $scope.fetchTypes();
        $scope.fetchCategories();
    }]);