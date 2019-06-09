import * as tslib_1 from "tslib";
import { Component } from "@angular/core";
import { DataService } from '../shared/dataService';
var Checkout = /** @class */ (function () {
    function Checkout(data) {
        this.data = data;
    }
    Checkout.prototype.onCheckout = function () {
        // TODO
        alert("Doing checkout");
    };
    Checkout = tslib_1.__decorate([
        Component({
            selector: "checkout",
            templateUrl: "checkout.component.html",
            styleUrls: ['checkout.component.css']
        }),
        tslib_1.__metadata("design:paramtypes", [DataService])
    ], Checkout);
    return Checkout;
}());
export { Checkout };
//# sourceMappingURL=checkout.component.js.map