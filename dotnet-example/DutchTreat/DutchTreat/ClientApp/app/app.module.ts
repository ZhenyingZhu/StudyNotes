import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { APP_BASE_HREF } from '@angular/common';

import { AppComponent } from './app.component';
import { ProductList } from "./shop/productlist.component";
import { Cart } from "./shop/cart.component";
import { DataService } from "./shared/dataService";
import { Shop } from "./shop/shop.component";
import { Checkout } from "./checkout/checkout.component";
import { Login } from "./login/login.component";

import { RouterModule } from "@angular/router";
import { FormsModule } from "@angular/forms";
import { take } from 'rxjs/operators';

let routes = [
    { path: "", component: Shop },
    { path: "checkout", component: Checkout },
    { path: "login", component: Login },
];

@NgModule({
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

export class AppModule { }
