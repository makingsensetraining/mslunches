import { Component, OnInit } from '@angular/core';
import { finalize } from 'rxjs/operators';

import { LaunchService } from './launch.service';
import { Launch } from '@app/launch/launch.model';

@Component({
  selector: 'app-home',
  templateUrl: './launch.component.html',
  styleUrls: ['./launch.component.scss']
})
export class LaunchComponent implements OnInit {

  launch: Launch;
  isLoading: boolean;

  constructor(private launchService: LaunchService) { }

  ngOnInit() {
    this.isLoading = true;
    this.launchService.getlaunches({ startDate: '2018-01-01', endDate: '2018-01-30'})
      .pipe(finalize(() => { this.isLoading = false; }))
      .subscribe((launches: Launch) => { this.launch = launch; });
  }

}
