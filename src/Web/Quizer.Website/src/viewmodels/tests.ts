import {useView} from 'aurelia-framework';
import {DataService} from '../dataAccess/dataService';
import {HttpClient} from 'aurelia-fetch-client';

@useView('views/tests.html')
export class Tests {
    heading = "Tests"

    constructor() {
        let dataService = new DataService();

        dataService.students.getAll();
    }
}