import { TestBed, ComponentFixture, inject, fakeAsync, tick } from '@angular/core/testing';
import { HttpTestingController, HttpClientTestingModule } from '@angular/common/http/testing';
import { LunchService } from '@app/lunch/lunch.service';
import { HttpClient } from '@angular/common/http';
import { CoreModule } from '@app/core';
import { RouterTestingModule } from '@angular/router/testing';
import { environment } from '@env/environment';

const routes = {
    getLunches: '/lunches',
    getGeneric(userId: string): string {
        return `/users/${userId}/lunches`;
    },
    getAccessor(userId: string, lunchId: string): string {
        return this.getGeneric(userId) + `/${lunchId}`;
    }
};

describe('LunchService', () => {
    let httpMock: HttpTestingController;
    let service: LunchService;

    beforeEach(() => {
        TestBed.configureTestingModule({
            imports: [
                CoreModule,
                HttpClientTestingModule,
                RouterTestingModule
            ],
            providers: [
                LunchService
            ]
        }).compileComponents();
    });

    beforeEach(inject([
        LunchService,
        HttpTestingController
    ], (_service: LunchService,
        _httpMock: HttpTestingController) => {
            service = _service;
            httpMock = _httpMock;
        })
    );

    it('should create', () => {
        expect(service).toBeTruthy();
    });

    it('getLunches should return lunches from enpoint', () => {
        service.getLunches().subscribe(l => {
            expect(l).toBeDefined();
            expect(l.length).toBeGreaterThan(0);
        });

        const req = httpMock.expectOne(environment.serverUrl + routes.getLunches);
        req.flush(mockedLunch, {
            status: 200,
            statusText: 'Ok'
        });
    });
});

const mockedSelection = [
    {
        lunchId: 'asd',
        id: 'us1'
    }
];

const mockedLunch = [
    {
        id: 'asd',
        meal: {
            name: 'mealName',
            mealType: {
                description: 'desc',
                isSelectable: true
            }
        },
        date: new Date('2018-06-05')
    }
];
