import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable, of } from "rxjs"
import { catchError, map } from 'rxjs/operators';
import { Product } from "./product";
import { Order} from "./order";
import { OrderItem } from "./orderItem";
import { DatePipe } from '@angular/common';

@Injectable()
export class ShopService {


    public order: Order = new Order();
    public products: Product[] = [];
    public memberships: Product[] = [];

    constructor(private http: HttpClient,
    private datePipe: DatePipe) {
    }

    private token: string = 'value';
    private tokenExpiration: Date = new Date('0001-01-01T00:00:00Z');


    public get SignInRequired(): boolean {
        return this.token === undefined ||
            this.token === 'value' ||
            this.token.length == 0 ||
            this.tokenExpiration >= new Date;
    }

    login(creds: any): Observable<boolean> {
        return this.http
            .post("/api/account/createtoken", creds)
            .pipe(map((data: any) => {
                this.token = data.token;
                this.tokenExpiration = data.expiry;
                return true;
            }));
    }

    getToken(): Observable<boolean> {
        return this.http
            .get("/api/account/getshoptoken")
            .pipe(map((data: any) => {
                this.token = data.token;
                this.tokenExpiration = data.expiry;
                return true;
            }), catchError(err => {
                return of(false);
            }));
    }

    public checkout() {

        console.log(this.token);

        console.log(JSON.stringify(this.order));

        this.order.dateCreated = new Date();
        this.order.orderNumber = this.datePipe.transform(new Date(), "yyyy-MM-dd_HH:mm:ss:SSS")?.toString()!;

        return this.http.post("/api/shop/orders", this.order, {
            headers: new HttpHeaders().set("Authorisation", "Bearer " + this.token)
        })
            .pipe(map(response => {
                console.log(JSON.stringify(response));
                this.order = new Order;
                return true;
                // add in page redirect
            }));
    }

    public clearCart() {
        this.order = new Order();
    }

    getProducts(): Observable<boolean> {
        return this.http.get("/api/shop/products")
            .pipe(map((data: any) => {
                this.products = data;
                return true;
            }));
    }

    getMemberships(): Observable<boolean> {
        return this.http.get("/api/shop/products/memberships")
            .pipe(map((data: any) => {
                this.memberships = data;
                return true;
            }));
    }

    public AddItemToOrder(product: Product) {

        let item: OrderItem = this.order.items.find(i => i.productId == product.productId)!;

        if (item) {
            item.quantity++;
        }
        else {
            item = new OrderItem();
            item.productId = product.productId;
            item.unitPrice = product.price;
            item.productName = product.name;
            item.productCategory = product.category;
            item.quantity = 1;
            item.imageId = product.imageId;

            this.order.items.push(item);
        }

    }
}