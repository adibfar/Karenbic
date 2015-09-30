App.controller('PortfoliosController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$upload', '$state',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal, $upload, $state) {
        $scope.types = [];
        $scope.categories = [];
        $scope.searchFields = {
            Type: null,
            Category: null,
        };
        $scope.portfolios = [];
        $scope.pages = [];
        $scope.pageCount = 0;
        $scope.pageIndex = 1;

        $scope.fetchTypes = function () {
            $scope.fetchTypesLoading = true;

            $http.get(baseUri + 'PortfolioType/Get')
            .success(function (data, status, headers, config) {
                $scope.types = data.Data;
                $scope.searchFields.Type = $scope.types[0];
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

        $scope.$watch(function () {
            return $scope.searchFields.Type;
        }, function (newValue, oldValue) {
            $scope.fetchCategories();
        });

        $scope.fetchCategories = function () {
            if ($scope.searchFields.Type == null) return;

            $scope.fetchCategoriesLoading = true;

            $http.get(baseUri + 'PortfolioCategory/Get', {
                params: {
                    typeId: $scope.searchFields.Type.Id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.categories = data.Data;
                $scope.searchFields.Category = $scope.categories[0];
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

        $scope.fetchPortfolios = function (pageIndex) {
            $scope.fetchLoading = true;

            $http.get(baseUri + 'Portfolio/Get', {
                params: {
                    typeId: $scope.searchFields.Type != null ? $scope.searchFields.Type.Id : null,
                    categoryId: $scope.searchFields.Category != null ? $scope.searchFields.Category.Id : null,
                    pageIndex: pageIndex
                }
            })
            .success(function (data, status, headers, config) {
                $scope.portfolios = data.Data.List;
                $scope.pageCount = data.Data.PageCount;
                $scope.pageIndex = data.Data.PageIndex;
                $scope.resultCount = data.Data.ResultCount;
                $scope.generatePagation();
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

        $scope.search = function () {
            $scope.pageIndex = 1;
            $scope.fetchPortfolios(1);
        };

        $scope.generatePagation = function () {
            $scope.pages = [];
            if ($scope.pageIndex - 2 > 0) $scope.pages.push($scope.pageIndex - 2);
            if ($scope.pageIndex - 1 > 0) $scope.pages.push($scope.pageIndex - 1);
            $scope.pages.push($scope.pageIndex);
            if ($scope.pageIndex + 1 <= $scope.pageCount) $scope.pages.push($scope.pageIndex + 1);
            if ($scope.pageIndex + 2 <= $scope.pageCount) $scope.pages.push($scope.pageIndex + 2);
        };

        $scope.changePage = function (index) {
            $scope.fetchPortfolios($scope.pages[index]);
        };

        $scope.nextPage = function () {
            if ($scope.pageIndex < $scope.pageCount && $scope.fetchLoading == false) {
                $scope.fetchPortfolios($scope.pageIndex + 1);
            }
        }

        $scope.prevPage = function () {
            if ($scope.pageIndex > 1 && $scope.fetchLoading == false) {
                $scope.fetchPortfolios($scope.pageIndex - 1);
            }
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
                    $http.post(baseUri + 'Portfolio/Remove',
                    {
                        id: $scope.portfolios[index].Id
                    }).
                    success(function (data, status, headers, config) {
                        $scope.fetchLoading = false;
                        if (data == "True") {
                            $scope.fetchPortfolios($scope.pageIndex);
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
            $state.go('^.portfolio-edit', { id: $scope.portfolios[index].Id });
        };

        //init
        $scope.fetchTypes();
        $scope.fetchPortfolios();
    }]);