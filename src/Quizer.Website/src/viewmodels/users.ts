import {useView} from 'aurelia-framework';

@useView('views/users.html')
export class Users {
    get heading() {
        return "Users";
    }
}