App.controller('PublicPriceController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$upload', '$state',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal, $upload, $state) {
        $scope.categories = [];
        $scope.selectedCategory = null;
        $scope.prices = [];

        $scope.fetchCategories = function () {
            $scope.fetchCategoriesLoading = true;
            $http.get(baseUri + 'PublicPriceCategory/Get')
            .success(function (data, status, headers, config) {
                $scope.categories = data;
                $scope.selectedCategory = $scope.categories[0];
                $scope.fetchCategoriesLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchCategoriesLoading = false;
            });
        };

        $scope.$watch(function () {
            return $scope.selectedCategory;
        }, function (newValue, oldValue) {
            if(newValue != null)
            {
                $scope.fetchPrices();
            }
        });

        $scope.fetchPrices = function () {
            if ($scope.selectedCategory == null) return;

            $scope.fetchLoading = true;

            $http.get(baseUri + 'PublicPrice/Get', {
                params: {
                    categoryId: $scope.selectedCategory.Id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.prices = data.Data;
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
                    $http.post(baseUri + 'PublicPrice/Remove',
                    {
                        id: $scope.prices[index].Id
                    }).
                    success(function (data, status, headers, config) {
                        $scope.fetchLoading = false;
                        if (data == "True") {
                            $scope.prices.splice(index, 1);
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

        $scope.edit = function (index) {
            $scope.fetchLoading = true;
            $state.go('^.public-price-edit', { id: $scope.prices[index].Id });
        };

        //init
        $scope.fetchCategories();
    }]);