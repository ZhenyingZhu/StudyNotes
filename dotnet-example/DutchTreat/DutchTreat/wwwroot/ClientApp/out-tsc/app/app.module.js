import * as tslib_1 from "tslib";
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { AppComponent } from './app.component';
import { ProductList } from "./shop/productlist.component";
import { Cart } from "./shop/cart.component";
import { DataService } from "./shared/dataService";
import { Shop } from "./shop/shop.component";
import { Checkout } from "./checkout/checkout.component";
import { Login } from "./login/login.component";
import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
var routes = [
    { path: "", component: Shop },
    { path: "checkout", component: Checkout },
    { path: "login", component: Login },
];
var AppModule = /** @class */ (function () {
    function AppModule() {
    }
    AppModule = tslib_1.__decorate([
        NgModule({
            declarations: [
                AppComponent,
                ProductList,
                Cart,
                Shop,
                Checkout,
                Login
            ],
            imports: [
                BrowserModule,
                HttpClientModule,
                FormsModule,
                RouterModule.forRoot(routes, {
                    useHash: true,
                    enableTracing: false // If want debugging, change it to true.
                }),
            ],
            providers: [
                DataService
            ],
            bootstrap: [AppComponent]
        })
    ], AppModule);
    return AppModule;
}());
export { AppModule };
//# sourceMappingURL=app.module.js.map