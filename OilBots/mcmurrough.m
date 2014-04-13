% Code by Chris McMurrough
% Swarm formation and collision avoidance simulation using potential fields

% Store the movie frame index for simulation video
frameNum = 1;

% For this example, vehicle(1) is the leader. All others follow while 
% avoiding eachother
numVehicles = 100;
vehiclePositions = [25 25; (50*(rand(numVehicles,2)-0.5))+ 25]; 
numVehicles = size(vehiclePositions);
vehicleVelocity = zeros(size(vehiclePositions));

% define the minimum distance between vehicles
proximity = 10;

% define the minimum distance from obstacles
Obstacleproximity = 10;

% define an acceptable distance to the goal
goalProximity = 2;

% store the environment points
world = [100,100];
             
% define the gains
K = 0.05;
KVehicle = K;
KLeader = K;
KObstacle = 2*K;
KGoal = K;
% define the seconds per iteration
T = 1;

% define the local minima detection variables
minimaCounter = 0;
minimaTimeout = 100;
minimaForce = 0.01; 

% preallocate space to store the force components for each vehicle
forces = zeros(size(vehiclePositions));

% store the distance of a point to a given object
r = 0;

% store the velocity components of a given vehicle
vx = 0;
vy = 0;

% make initial plot
plot(vehiclePositions(:,1),vehiclePositions(:,2),['b','o']);
axis([0,world(1,1),0,world(1,2)]);
hold on;

% get the obstacles
obstacles = ginput;
numObstacles = size(obstacles);
numObstacles = numObstacles(1);

% plot obstacles before setting waypoints
plot(obstacles(:,1),obstacles(:,2),['r','o']);
plot(vehiclePositions(1,1),vehiclePositions(1,2),['g','o']);
pause(1);
hold off;

% get the waypoints
waypoints = ginput;
numWaypoints = size(waypoints);
numWaypoints = numWaypoints(1);
waypointIndex = 1;

while waypointIndex <= numWaypoints
    
    % check for local minima
    % start the counter if the last force was too small   
    if sum(abs(forces(1,:))) < minimaForce
        minimaCounter = minimaCounter + 1;
    else
        minimaCounter = 0;
    end
    % if the minima counter has timed out, elect a new leader
    if minimaCounter > minimaTimeout
        % select the new leader as the vehicle with greatest net force
        [Y,I] = max(abs(forces(:,1)) + abs(forces(:,2)));
        % swap the current leader and the newly elected leader
        temp = vehiclePositions(1,:);
        vehiclePositions(1,:) = vehiclePositions(I,:);
        vehiclePositions(I,:) = temp;
    end
    
    forces(:,:) = 0;
    forces(:,:) = 0;
    % calculate the goal attractive force components
    for i = 1:1
        x1 = vehiclePositions(i,1);
        x2 = waypoints(waypointIndex,1);
        y1 = vehiclePositions(i,2);
        y2 = waypoints(waypointIndex,2);
        r = (x2-x1)^2 + (y2-y1)^2;
        r = sqrt(r);
        % if you are within range of the goal, incrememnt the waypoint
        % counter
        if r < goalProximity
            waypointIndex = waypointIndex + 1;
        end
        if x2 > x1
            forces(i,1) = forces(i,1) + KGoal*r*cos(atan((y2-y1)/(x2-x1)));
            forces(i,2) = forces(i,2) + KGoal*r*sin(atan((y2-y1)/(x2-x1)));
        else
            forces(i,1) = forces(i,1) - KGoal*r*cos(atan((y2-y1)/(x2-x1)));
            forces(i,2) = forces(i,2) - KGoal*r*sin(atan((y2-y1)/(x2-x1)));
        end

    end
    % calculate the obstacle repulsive force components
    for i = 1:numVehicles
        for j = 1:numObstacles
            x1 = vehiclePositions(i,1);
            x2 = obstacles(j,1);
            y1 = vehiclePositions(i,2);
            y2 = obstacles(j,2);
            r = (x2-x1)^2 + (y2-y1)^2;
            r = sqrt(r);
            if r < proximity 
                if x2 > x1
                    forces(i,1) = forces(i,1) - KObstacle*(Obstacleproximity-r)^2*cos(atan((y2-y1)/(x2-x1)));
                    forces(i,2) = forces(i,2) - KObstacle*(Obstacleproximity-r)^2*sin(atan((y2-y1)/(x2-x1)));
                else
                    forces(i,1) = forces(i,1) + KObstacle*(Obstacleproximity-r)^2*cos(atan((y2-y1)/(x2-x1)));
                    forces(i,2) = forces(i,2) + KObstacle*(Obstacleproximity-r)^2*sin(atan((y2-y1)/(x2-x1)));
                end
            end
        end
    end
    % calculate the vehicle on vehicle force components
    % the leader is not affected by others, so start iteration at 2
    for i = 2:numVehicles
        for j = 1:numVehicles
            if i ~= j
                x1 = vehiclePositions(i,1);
                x2 = vehiclePositions(j,1);
                y1 = vehiclePositions(i,2);
                y2 = vehiclePositions(j,2);
                r = (x2-x1)^2 + (y2-y1)^2;
                r = sqrt(r);
                % the leader exerts an attractive force on others 
                if j == 1
                    if r < proximity
                        if x2 > x1
                            forces(i,1) = forces(i,1) - KLeader*(proximity-r)^2*cos(atan((y2-y1)/(x2-x1)));
                            forces(i,2) = forces(i,2) - KLeader*(proximity-r)^2*sin(atan((y2-y1)/(x2-x1)));
                        else
                            forces(i,1) = forces(i,1) + KLeader*(proximity-r)^2*cos(atan((y2-y1)/(x2-x1)));
                            forces(i,2) = forces(i,2) + KLeader*(proximity-r)^2*sin(atan((y2-y1)/(x2-x1)));
                        end
                    else
                        if x2 > x1
                            forces(i,1) = forces(i,1) + KLeader*r*cos(atan((y2-y1)/(x2-x1)));
                            forces(i,2) = forces(i,2) + KLeader*r*sin(atan((y2-y1)/(x2-x1)));
                        else
                            forces(i,1) = forces(i,1) - KLeader*r*cos(atan((y2-y1)/(x2-x1)));
                            forces(i,2) = forces(i,2) - KLeader*r*sin(atan((y2-y1)/(x2-x1)));
                        end
                    end
                else
                    if r < proximity
                        if x2 > x1
                            forces(i,1) = forces(i,1) - KVehicle*(proximity-r)^2*cos(atan((y2-y1)/(x2-x1)));
                            forces(i,2) = forces(i,2) - KVehicle*(proximity-r)^2*sin(atan((y2-y1)/(x2-x1)));
                        else
                            forces(i,1) = forces(i,1) + KVehicle*(proximity-r)^2*cos(atan((y2-y1)/(x2-x1)));
                            forces(i,2) = forces(i,2) + KVehicle*(proximity-r)^2*sin(atan((y2-y1)/(x2-x1)));
                        end
                    %else
                        %if x2 > x1
                        %    forces(i,1) = forces(i,1) + KVehicle*r*cos(atan((y2-y1)/(x2-x1)));
                        %    forces(i,2) = forces(i,2) + KVehicle*r*sin(atan((y2-y1)/(x2-x1)));
                        %else
                        %    forces(i,1) = forces(i,1) - KVehicle*r*cos(atan((y2-y1)/(x2-x1)));
                        %    forces(i,2) = forces(i,2) - KVehicle*r*sin(atan((y2-y1)/(x2-x1)));
                        %end
                    end
                end
            end
        end
    end
    
    % update velocities and positions using kinematic equations
    % d = v0t + 1/2at^2
    % v = v0 + at
    %forces(1,:) = [0 0];
    for i = 1:numVehicles
        vehiclePositions(i,1) = vehiclePositions(i,1) * T + 0.5 * forces(i,1) * T^2;
        vehiclePositions(i,2) = vehiclePositions(i,2) * T + 0.5 * forces(i,2) * T^2;
        vehicleVelocity(i,1) = vehicleVelocity(i,1) + forces(i,1) * T;
        vehicleVelocity(i,2) = vehicleVelocity(i,2) + forces(i,2) * T;
    end
   
    % plot all of the vehicle positions, waypoints, and obstacles
    plot(vehiclePositions(:,1),vehiclePositions(:,2),['b','o']);
    axis([0,world(1,1),0,world(1,2)]); 
    hold on;
    plot(obstacles(:,1),obstacles(:,2),['black','o']);
    plot(vehiclePositions(1,1),vehiclePositions(1,2),['g','*']);
    plot(waypoints(:,1),waypoints(:,2),['r','+']);
    pause(0.001);
    hold off;
    MOVIE(frameNum) = getframe;
    frameNum = frameNum + 1;
end

% save the movie file
movie2avi(MOVIE,'movie.avi','compression','Cinepak', 'fps', 60)