import { Injectable } from '@angular/core';
import { Observable } from 'rxjs/Observable';
import { map, mapTo } from 'rxjs/operators';
import { Launch } from '@app/launch/launch.model';
import { HttpClient } from '@angular/common/http';
import { of } from 'rxjs/observable/of';

@Injectable()
export class LaunchService {
    constructor(private httpClient: HttpClient) {
    }


    getLaunches(startDate: Date, endDate: Date): Observable<Array<Launch>> {
        /*return this.httpClient
            .get('/Launch')
            .pipe(map(this.mapToArrayOfLaunch));*/
        return of(this.mapToArrayOfLaunch(null));
    }

    private mapToArrayOfLaunch(body: any): Array<Launch> {
        const result = new Array<Launch>();
        for (let i = 14; i < 26; i++) {
            if (i !== 19 && i !== 20) {
                result.push({
                    type: 'calorico',
                    date: new Date(2018, 5, i),
                    description: 'pollo con papas'});
            }
        }
        return result;
        /*
        let result: Array<Launch> = new Array<Launch>();
        const json: any = JSON.parse(body);

        result = json.forEach((element: any) => {
            return this.mapToLaunch(element);
        });
        return result;*/
    }

    private mapToLaunch(body: any): Launch {
        let result: Launch;
        result = {
            description: body.description,
            type: body.type,
            date: body.date
        };

        return result;
    }
}
