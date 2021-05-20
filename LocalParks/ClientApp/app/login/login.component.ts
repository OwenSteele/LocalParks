import { Component } from "@angular/core";
import { ShopService } from "../data/shopService";
import { Router } from "@angular/router";

@Component({
    selector: "login",
    templateUrl: "login.component.html",
    styleUrls: []
})

export class Login {


    constructor(private data: ShopService, private router: Router) {

    }

    errorMessage: string = "";
    public creds = {
        username: "",
        password: ""
    }

    onLogin() {
        this.data.login(this.creds)
            .subscribe(success => {
                if (this.data.order.items.length == 0) {
                    this.router.navigate([""]);
                }
                else {
                    this.router.navigate(["checkout"]);
                }
            }, err => this.errorMessage = "Failed to login.");
    }

}