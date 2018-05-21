import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { map, mapTo } from 'rxjs/operators';
import { Launch } from '@app/launch/launch.model';
import { HttpClient } from '@angular/common/http';

@Injectable()
export class LaunchService {
    constructor(private httpClient: HttpClient) {
    }


    getLaunches(startDate: Date, endDate: Date): Observable<Array<Launch>> {
        return this.httpClient
            .get('/Launch')
            .pipe(map(this.mapToArrayOfLaunch));
    }

    private mapToArrayOfLaunch(body: any): Array<Launch> {
        let result: Array<Launch> = new Array<Launch>();
        const json: any = JSON.parse(body);

        result = json.forEach((element: any) => {
            return this.mapToLaunch(element);
        });
        return result;
    }

    private mapToLaunch(body: any): Launch {
        let result: Launch;
        result = {
            description: body.description,
            type: body.type
        };

        return result;
    }
}
