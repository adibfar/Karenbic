App.controller('PortfoliosController', ['$scope', '$http', 'ngDialog', 'APP_BASE_URI', 'toaster', '$modal', '$upload',
    function ($scope, $http, ngDialog, baseUri, toaster, $modal, $upload) {
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

        $scope.showEditModal = function (index) {
            var modalInstance = $modal.open({
                templateUrl: '/EditModalContent.html',
                controller: EditCtrl,
                size: 'lg',
                resolve: {
                    types: function () {
                        return _.clone($scope.types);
                    },
                    portfolio: function () {
                        var portfolio = {
                            Id: $scope.portfolios[index].Id,
                            Priority: $scope.portfolios[index].Priority,
                            Description: $scope.portfolios[index].Description,
                            TumbPictureFile: $scope.portfolios[index].TumbPictureFile,
                            TumbPicturePath: $scope.portfolios[index].TumbPicturePath,
                            PictureFile: $scope.portfolios[index].PictureFile,
                            PicturePath: $scope.portfolios[index].PicturePath,
                            Category: {
                                Id: $scope.portfolios[index].Category.Id,
                                Title: $scope.portfolios[index].Category.Title,
                                Type: {
                                    Id: $scope.portfolios[index].Category.Type.Id,
                                    Title: $scope.portfolios[index].Category.Type.Title
                                }
                            }
                        };
                        return portfolio;
                    }
                }
            });

            modalInstance.result.then(function (result) {
                $scope.fetchPortfolios($scope.pageIndex);
            }, function () {
            });
        };

        var EditCtrl = ['$scope', '$http', '$modalInstance', 'types', 'portfolio', function ($scope, $http, $modalInstance, types, portfolio) {

            $scope.types = types;
            $scope.categories = [];
            $scope.portfolio = portfolio;
            $scope.newPictureFile = null;
            $scope.newTumbPictureFile = null;
            $scope.firstFetchCategory = true;

            var typeIndex = _.findIndex($scope.types, function (item) {
                return item.Id = $scope.portfolio.Category.Type.Id;
            });
            $scope.portfolio.Type = $scope.types[typeIndex];

            $scope.$watch(function () {
                return $scope.portfolio.Type;
            }, function (newValue, oldValue) {
                $scope.fetchCategories();
            });

            $scope.fetchCategories = function () {
                if ($scope.portfolio.Type == null) return;

                $scope.fetchCategoriesLoading = true;

                $http.get(baseUri + 'PortfolioCategory/Get', {
                    params: {
                        typeId: $scope.portfolio.Type.Id
                    }
                })
                .success(function (data, status, headers, config) {
                    $scope.categories = data.Data;
                    if ($scope.firstFetchCategory == true) {
                        var categoryIndex = _.findIndex($scope.categories, function (item) {
                            return item.Id = $scope.portfolio.Category.Id;
                        });
                        $scope.portfolio.Category = $scope.categories[categoryIndex];
                        $scope.firstFetchCategory = false;
                    }
                    else {
                        $scope.portfolio.Category = $scope.categories[0];
                    }
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

            $scope.onPictureSelect = function ($files) {
                var $file = $files[0];

                if ($file.type != 'image/jpeg' && $file.type != 'image/jpg' && $file.type != 'image/png') {
                    toaster.pop('error', "فرمت فایل تصویر باید jpg یا png باشد");
                    return;
                }

                if ($file.size > 250 * 1024) {
                    toaster.pop('error', "حداکثر حجم فایل 250 کیلو بایت می باشد");
                    return;
                }

                $scope.newPictureFile = $file;

                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#pic').attr('src', e.target.result);
                }
                reader.readAsDataURL($file);
            };

            $scope.onTumbPictureSelect = function ($files) {
                var $file = $files[0];

                if ($file.type != 'image/jpeg' && $file.type != 'image/jpg' && $file.type != 'image/png') {
                    toaster.pop('error', "فرمت فایل تصویر باید jpg یا png باشد");
                    return;
                }

                if ($file.size > 250 * 1024) {
                    toaster.pop('error', "حداکثر حجم فایل 250 کیلو بایت می باشد");
                    return;
                }

                $scope.newTumbPictureFile = $file;

                var reader = new FileReader();
                reader.onload = function (e) {
                    $('#tumbPic').attr('src', e.target.result);
                }
                reader.readAsDataURL($file);
            };

            $scope.editFormIsValide = function () {
                if ($scope.portfolio.Type == null ||
                    $scope.portfolio.Category == null ||
                    isNaN($scope.portfolio.Priority))
                    return false;
                return true;
            };

            $scope.edit = function () {
                if ($scope.editFormIsValide() == false) return;
                $scope.editLoading = true;
                if ($scope.newPictureFile == null && $scope.newTumbPictureFile == null) $scope.edit_1();
                else $scope.edit_2();
            };

            $scope.edit_1 = function () {
                $http.post(baseUri + 'Portfolio/Edit',
                {
                    id: $scope.portfolio.Id,
                    categoryId: $scope.portfolio.Category.Id,
                    priority: $scope.portfolio.Priority,
                    description: $scope.portfolio.Description
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

            $scope.edit_2 = function () {
                var file = [];
                var fileName = [];

                if ($scope.newPictureFile != null) {
                    file.push($scope.newPictureFile);
                    fileName.push("picture");
                }

                if ($scope.newTumbPictureFile != null) {
                    file.push($scope.newTumbPictureFile);
                    fileName.push("tumbPicture");
                }

                console.log(fileName);

                $upload.upload({
                    url: baseUri + 'Portfolio/Edit',
                    file: file,
                    fileFormDataName: fileName,
                    data: {
                        id: $scope.portfolio.Id,
                        categoryId: $scope.portfolio.Category.Id,
                        priority: $scope.portfolio.Priority,
                        description: $scope.portfolio.Description
                    }
                }).success(function (data, status, headers, config) {
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
        $scope.fetchPortfolios();
    }]);