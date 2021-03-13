import { Component } from "@angular/core";
import { ShopService } from "../data/shopService";
import { Router } from "@angular/router";

@Component({
    selector: "user-cart",
    templateUrl: "cart.component.html",
    styleUrls: []
})
export class Cart {
    constructor(public data: ShopService,
        private router: Router) { }

    onCheckout() {
        if (this.data.IsSignedIn) {
            console.log("issigned in");
            this.router.navigate([""]);
        }
        else {
            this.data.getToken()
                .subscribe(success => {
                    if (success) {
                        this.router.navigate([""]);
                    }
                    else {
                        this.router.navigate(["login"]);
                    }
                })
        }
    }
}