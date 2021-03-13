import { HttpClient, HttpHeaders } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs"
import { map } from 'rxjs/operators';
import { Product } from "./product";
import { Order} from "./order";
import { OrderItem } from "./orderItem";

@Injectable()
export class ShopService {

    private token: string = "";
    private tokenExpiration: Date;

    public order: Order = new Order();
    public products: Product[] = [];
    public memberships: Product[] = [];

    constructor(private http: HttpClient) {
        this.token = "";
    }

    public get IsSignedIn(): boolean {
        return this.token != "" ||
            this.token.length > 0 ||
            this.tokenExpiration <= new Date;
    }

    login(creds: any): Observable<boolean> {
        return this.http.post("/api/account/createtoken", creds)
            .pipe(map((data: any) => {
                this.token = data[0];
                this.tokenExpiration = data[1];
                return true;
            }));
    }

    getToken(): Observable<boolean> {
        return this.http.get("/api/account/createtoken")
            .pipe(map((data: any) => {
                this.token = data[0];
                this.tokenExpiration = data[1];
                return true;
            }));
    }

    public checkout() {
        return this.http.post("/api/shop/orders", this.order, {
            headers: new HttpHeaders().set("Authorisation", "Bearer " + this.token)
        })
            .pipe(map(response => {
                this.order = new Order;
                return true;
                // add in page redirect
            }));
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