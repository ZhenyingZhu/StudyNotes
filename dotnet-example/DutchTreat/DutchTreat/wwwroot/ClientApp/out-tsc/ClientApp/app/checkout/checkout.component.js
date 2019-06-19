import * as tslib_1 from "tslib";
import { Component } from "@angular/core";
import { DataService } from '../shared/dataService';
import { Router } from '@angular/router';
var Checkout = /** @class */ (function () {
    function Checkout(data, router) {
        this.data = data;
        this.router = router;
        this.errorMessage = "";
    }
    Checkout.prototype.onCheckout = function () {
        var _this = this;
        // TODO
        //alert("Doing checkout");
        this.data.checkout()
            .subscribe(function (success) {
            if (success) {
                _this.router.navigate(["/"]);
            }
        }, function (err) { return _this.errorMessage = "Failed to save order"; });
    };
    Checkout = tslib_1.__decorate([
        Component({
            selector: "checkout",
            templateUrl: "checkout.component.html",
            styleUrls: ['checkout.component.css']
        }),
        tslib_1.__metadata("design:paramtypes", [DataService, Router])
    ], Checkout);
    return Checkout;
}());
export { Checkout };
//# sourceMappingURL=checkout.component.js.map