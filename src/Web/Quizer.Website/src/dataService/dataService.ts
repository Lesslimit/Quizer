import {HttpClient} from 'aurelia-fetch-client';
import {autoinject} from 'aurelia-framework';

@autoinject()
export class DataService {
    httpClient: HttpClient;

    constructor(httpClient: HttpClient) {
          this.httpClient = httpClient;
     }

     configure(baseUrl) {
         this.httpClient.configure(config => {
             config
                 .withBaseUrl(baseUrl)
                 .withDefaults({
                     credentials: 'same-origin',
                     headers: {
                         'Accept': 'application/json',
                         'X-Requested-With': 'Fetch'
                     }
                 })
                 .withInterceptor({
                     request(request) {
                         console.log(`Requesting ${request.method} ${request.url}`);
                         return request;
                     },
                     response(response) {
                         console.log(`Received ${response.status} ${response.url}`);
                         return response;
                     }
                 });
         });
     }

     get students() {
        const ds = this;

        return {
            getAll() {
                ds.httpClient.fetch('students/getAll').then(data => { });
            }
        }
    }

     get tests() {
        const ds = this;

        return {
            getAll() {
                ds.httpClient.fetch('tests/getAll').then(data => { });
            }
        }
    }
}