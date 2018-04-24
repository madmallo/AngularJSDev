function AngularInit(callback_ok, callback_error) {
    var a = angular.module("aBrowser", []);
    a.controller("cBrowser", function ($scope, $http) {
        GetData($scope, $http, callback_ok, callback_error);
    });
}

function GetData($scope, $http, callback_ok, callback_error) {
    /*var json = json_return;
    $scope.BrowserData = json_return;
    AddStyles(json);
    callback_ok();*/
    callback_error = (callback_error == null) ? (function () {  }) : callback_error;
    $http({
        method: "POST",
        url: "/Browser/GetData/",
        headers: {
            "Content-Type": "application/json"
        },
        data: {}
    })
        .then(function (response) {
            var json = response.data;
            $scope.BrowserData = json;
            AddStyles(json);
            callback_ok();
        }, function (response) {
            alert(response.data);
            callback_error();
        });
}

function AddStyles(json) {
    var sBrowserStyle = "";
    var sum_width = 0, col_width;
    for (var i = 0; i < json.head.length; i++) {
        col_width = json.head[i].width;
        if (col_width != "") { sum_width += parseFloat(col_width.replace("px", "")); }
        sBrowserStyle += " ." + json.head[i].nome + "{"
            + ((col_width != "") ? (" width: " + col_width + "; ") : " width: [WIDTH_PLACEHOLDER]; ")
            + " text-align: " + json.head[i].align + "; "
            + "} ";
    }
    var cnt_placeholders = (sBrowserStyle.match(/\[WIDTH_PLACEHOLDER\]/g) || []).length;
    if (cnt_placeholders != 0) {
        sBrowserStyle = sBrowserStyle.replace(/\[WIDTH_PLACEHOLDER\]/g, (($("#test_browser table").width() - sum_width) / cnt_placeholders) + "px");
    }
    $("#BrowserStyle").html(sBrowserStyle);
}

function Browserify(d) {
    if (!d.hasClass("lteIE9")) {
        d.find("tbody").css({ height: ((d.height() - d.find("thead").height() - 1) + "px") });
    }
}

/*
string ret = "{"
           +    "head: ["
           +        "{"
           +            "nome: \"Colonna1\","
           +            "caption: \"Col 1\","
           +            "tipo: \"string\","
           +            "width: \"250px\","
           +            "align: \"left\","
           +            "format: \"\""
           +        "},"
           +        "{"
           +            "nome: \"Colonna2\","
           +            "caption: \"Col 2\","
           +            "tipo: \"string\","
           +            "width: \"\","
           +            "align: \"left\","
           +            "format: \"\""
           +        "},"
           +        "{"
           +            "nome: \"Colonna3\","
           +            "caption: \"Col 3\","
           +            "tipo: \"string\","
           +            "width: \"\","
           +            "align: \"left\","
           +            "format: \"\""
           +        "},"
           +        "{"
           +            "nome: \"Colonna4\","
           +            "caption: \"Col 4\","
           +            "tipo: \"number\","
           +            "width: \"100px\","
           +            "align: \"right\","
           +            "format: \"\""
           +        "},"
           +        "{"
           +            "nome: \"Colonna5\","
           +            "caption: \"Col 5\","
           +            "tipo: \"datetime\","
           +            "width: \"100px\","
           +            "align: \"center\","
           +            "format: \"\""
           +        "}"
           +    "],"
           +    "body: ["
           +        "{"
           +            "Colonna1: \"uallabalooza\","
           +            "Colonna2: \"in the\","
           +            "Colonna3: \"sky\","
           +            "Colonna4: 245.1,"
           +            "Colonna5: \"01/01/9999\""
           +        "},"
           +        "{"
           +            "Colonna1: \"rasti\","
           +            "Colonna2: \"blasti\","
           +            "Colonna3: \"lasagne\","
           +            "Colonna4: -14000,"
           +            "Colonna5: \"01/01/9999\""
           +        "},"
           +        "{"
           +            "Colonna1: \"similili\","
           +            "Colonna2: \"la formica\","
           +            "Colonna3: \"brombagliumbal\","
           +            "Colonna4: 12,"
           +            "Colonna5: \"01/01/9999\""
           +        "}"
           +    "]"
           +"}";
*/