App.controller('PriceListController', ['$scope', '$http', 'APP_BASE_URI', 'toaster', '$modal', '$sce',
    function ($scope, $http, baseUri, toaster, $modal, $sce) {
        $scope.categories = [];
        $scope.selectedCategory = {};
        $scope.priceLists = [];

        $scope.fetchPriceCategories = function () {
            $scope.fetchPriceCategoriesLoading = true;

            $http.get(baseUri + 'PriceList/GetCategory')
            .success(function (data, status, headers, config) {
                $scope.categories = data;
                $scope.categories.unshift({
                    Id: 0,
                    Title: 'لطفاً یکی از گزینه ها را انتخاب کنید'
                });
                $scope.selectedCategory = $scope.categories[0];
                $scope.fetchPriceCategoriesLoading = false;
            }).error(function (data, status, headers, config) {
                if (status == 403) {
                    window.location = "/Account/Login";
                }
                else {
                    toaster.pop('error', "خطایی رخ داده صفحه را مجدداً بارگزاری کنید");
                }
                $scope.fetchPriceCategoriesLoading = false;
            });
        };

        $scope.fetchPriceLists = function () {
            if ($scope.selectedCategory.Id == undefined ||
                $scope.selectedCategory.Id == null ||
                $scope.selectedCategory.Id == 0) return;

            $scope.fetchLoading = true;

            $http.get(baseUri + 'PriceList/Get', {
                params: {
                    categoryId: $scope.selectedCategory.Id
                }
            })
            .success(function (data, status, headers, config) {
                $scope.priceLists = data;
                _.each($scope.priceLists, function (item) {
                    item.Description2 = $sce.trustAsHtml(item.Description);
                });
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

        $scope.PriceListClicked = function ($event, item) {
            var $this = $($event.currentTarget);

            if ($($this).closest('.accordion-row').hasClass('expand')) {
                $($this).closest('.accordion-row').find('.desc').stop(true, true).slideUp(300);
                $($this).closest('.accordion-row').find('.close-icon').removeClass('close-icon').addClass('open-icon');
                $($this).closest('.accordion-row').removeClass('expand');
            }
            else {
                if ($('.expand').length > 0) {
                    var $expandedItem = $('.expand').closest('.accordion-row');
                    $($expandedItem).removeClass('expand');
                    $($expandedItem).find('.desc').stop(true, true).slideUp(300);
                    $($expandedItem).find('.close-icon').removeClass('close-icon').addClass('open-icon');
                }
                $($this).closest('.accordion-row').find('.desc').slideDown(300);
                $($this).closest('.accordion-row').find('.open-icon').removeClass('open-icon').addClass('close-icon');
                $($this).closest('.accordion-row').addClass('expand');
            }
        };

        //init
        $scope.fetchPriceCategories();
    }]);