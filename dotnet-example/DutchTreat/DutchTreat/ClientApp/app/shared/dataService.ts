import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { Product } from './product';
import * as OrderNS from './order';

@Injectable()
export class DataService {
    // Replace those example data with calling API.
    //public products = [
    //    {
    //        title: "First product",
    //        price: 19.99
    //    },
    //    {
    //        title: "Second product",
    //        price: 9.99
    //    },
    //    {
    //        title: "Third product",
    //        price: 14.99
    //    },
    //];

    constructor(private http: HttpClient) {
    }

    private token: string = "";
    private tokenExpiration: Date;


    public order: OrderNS.Order = new OrderNS.Order();

    public products: Product[] = [];

    loadProducts(): Observable<boolean> {
        return this.http.get("api/products")
            .pipe(map((data: any[]) => {
                this.products = data;
                return true;
            }));
    }

    public get loginRequired(): boolean {
        return this.token.length == 0 || this.tokenExpiration > new Date();
    }

    login(creds): Observable<boolean> {
        return this.http.post("account/createtoken", creds)
            .pipe(map((data: any) => {
                this.token = data.token;
                this.tokenExpiration = data.expriation;
                return true;
            }));
    }

    public addToOrder(newProduct: Product) {
        var item: OrderNS.OrderItem = this.order.items.find(i => i.productId == newProduct.id);

        if (item) {
            item.quantity++;
        } else {
            item = new OrderNS.OrderItem();
            item.productId = newProduct.id;
            item.productArtist = newProduct.artist;
            item.productArtId = newProduct.artId;
            item.productCategory = newProduct.category;
            item.productSize = newProduct.size;
            item.productTitle = newProduct.title;
            item.unitPrice = newProduct.price;
            item.quantity = 1;

            this.order.items.push(item);
        }

    }
}