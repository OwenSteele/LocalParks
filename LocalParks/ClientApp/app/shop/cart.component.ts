import { Component, EventEmitter, Output } from "@angular/core";
import { ShopService } from "../data/shopService";
import { Router } from "@angular/router";
import { OrderItem } from "../data/orderItem";

@Component({
    selector: "user-cart",
    templateUrl: "cart.component.html",
    styleUrls: ["cart.component.css"]
})
export class Cart {
    constructor(public data: ShopService,
        private router: Router) { }

    @Output() close: EventEmitter<string> = new EventEmitter();

    removeItem(item: OrderItem) {
        this.data.RemoveItemFromOrder(item);
    }
    onCheckout() {
        if (this.data.SignInRequired) {
            this.data.getToken()
            .subscribe(success => {
                if (success) {
                    this.router.navigate(["checkout"]);
                }
                else {
                    this.router.navigate(["login"]);
                }
            })
        }
        else {
            this.router.navigate([""]);            
        }
    }
    closeNav() {
        console.log("close");
        this.close.emit("close");
    }
}