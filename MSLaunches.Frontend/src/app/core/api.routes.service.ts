import { Injectable } from '@angular/core';

@Injectable()
export class ApiRoutesService {

    getMeals(): string {
        return '/meals';
    }

    getMealTypes(): string {
        return '/mealtypes';
    }

    batchSaveLunches(): string {
        return '/lunches/batchsave';
    }

    getUserLunches(userId: string, startdate?: Date, enddate?: Date): string {
        let uri = `/users/${userId}/lunches`;
        if (!!startdate) {
            uri = this.addQueryParam(uri, 'startDate', startdate.toISOString());
        }
        if (!!enddate) {
            uri = this.addQueryParam(uri, 'endDate', enddate.toISOString());
        }
        return uri;
    }

    getLunches(startdate?: Date, enddate?: Date): string {
        let uri = '/lunches';
        if (!!startdate) {
            uri = this.addQueryParam(uri, 'startDate', startdate.toISOString());
        }
        if (!!enddate) {
            uri = this.addQueryParam(uri, 'endDate', enddate.toISOString());
        }
        return uri;
    }

    userLunchResource(userid: string, id?: string): string {
        let uri = `/users/${userid}/lunches`;
        if (id) {
            uri += `/${id}`;
        }
        return uri;
    }

    private addQueryParam(uri: string, name: string, value: string): string {
        const queryParam = name + '=' + value;
        let result = '';
        if (uri.includes('?')) {
            result = uri + '&' + queryParam;
        } else {
            result = uri + '?' + queryParam;
        }

        return result;
    }
}
