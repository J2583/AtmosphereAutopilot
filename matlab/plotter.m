%% data
run('import_telemetry');
%% plot

scrsz = get(0,'ScreenSize');
figure('Name','Telemetry Plotter',...
    'Position',[100 50 scrsz(3)*0.9 scrsz(4)*0.8])
hold on
%plot(time,acc,'r','Marker','.','MarkerSize',5)
plot(time,predict,'r','Marker','.','MarkerSize',5)
plot(time,control,'k','Marker','.','MarkerSize',5)
plot(time,aoa,'b','Marker','.','MarkerSize',5)
plot(time,v,'g')
hold off
xlabel('time')
legend('predict','control','aoa','v');
h = gca;
set(h, 'Position', [0.025 0.06 0.96 0.92]);