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

  launches: Array<Launch>;
  isLoading: boolean;

  constructor(private launchService: LaunchService) { }

  ngOnInit() {
    this.isLoading = true;
    this.launchService.getLaunches(new Date(2018, 1, 1), new Date(2018, 1, 0))
      .pipe(finalize(() => { this.isLoading = false; }))
      .subscribe(launches => { this.launches = launches; });
  }

}
