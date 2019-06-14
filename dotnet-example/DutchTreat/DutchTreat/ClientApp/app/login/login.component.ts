import { Component } from "@angular/core";
import { DataService } from '../shared/dataService';
import { Router } from '@angular/router';

@Component({
    selector: "login",
    templateUrl: "login.component.html"
})
export class Login {

    constructor(private data: DataService, private router: Router) { }

    public creds = {
        username: "",
        password: ""
    };

    onLogin() {
        //alert(this.creds.username);
        // this.creds.username += "!"; // this change should push back.
    }
}