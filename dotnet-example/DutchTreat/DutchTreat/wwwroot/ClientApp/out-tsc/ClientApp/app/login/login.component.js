import * as tslib_1 from "tslib";
import { Component } from "@angular/core";
import { DataService } from '../shared/dataService';
import { Router } from '@angular/router';
var Login = /** @class */ (function () {
    function Login(data, router) {
        this.data = data;
        this.router = router;
        this.creds = {
            username: "",
            password: ""
        };
    }
    Login.prototype.onLogin = function () {
        alert(this.creds.username);
    };
    Login = tslib_1.__decorate([
        Component({
            selector: "login",
            templateUrl: "login.component.html"
        }),
        tslib_1.__metadata("design:paramtypes", [DataService, Router])
    ], Login);
    return Login;
}());
export { Login };
//# sourceMappingURL=login.component.js.map