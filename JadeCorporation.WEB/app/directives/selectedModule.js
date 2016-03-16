app.directive('selectedModule', function () {
    return {
        restrict: 'A',

        link: function (scope, elem, attrs) {
            var currentState = true;
            elem.on('click', function () {
                console.log('You clicked me!');

                if (currentState === true) {
                    console.log('It is on!');
                    //angular.element(elem).removeClass("");
                    angular.element(elem).addClass("nav-active");
                    //console.log(elem[0].querySelector('.child-list'));

                } else {
                    console.log('It is off!');
                    angular.element(elem).removeClass("nav-active");
                    //angular.element(elem).addClass(attrs.onIcon);
                }

                currentState = !currentState

            });


        }
    };
});