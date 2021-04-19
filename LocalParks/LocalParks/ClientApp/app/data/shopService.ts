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

        this.order.dateCreated = new Date();
        console.log("orderno set");
        this.order.orderNumber = this.datePipe.transform(new Date(), "yyyyMMddHHmmssSSS")?.toString()!;
        console.log(this.order.orderNumber);

        return this.http.post("/api/shop/orders", this.order, {
            headers: new HttpHeaders({
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${this.token}`
            })
        })
            .pipe(map(response => {
                return true;
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

    public RemoveItemFromOrder(item: OrderItem) {
        let itemToRemove: OrderItem = this.order.items.find(i => i.productId == item.productId)!;

        if (itemToRemove) {
            if (itemToRemove.quantity === 1) {
                let index: number = this.order.items.indexOf(itemToRemove, 0);
                this.order.items.splice(index, 1);
            }
            else {
                itemToRemove.quantity--;
            }
        }
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