unit IntegralCalculationUnit; //������������ ���������� ���������

interface
  uses PointsArrayUnit;
  const LEFT_RECTANGLES_FROMULA = 0; CENTRAL_RECTANGLES_FROMULA = 1; TRAPEZOIDS_FROMULA = 2;
        SIMPSON_FROMULA = 3; GAUSS_FROMULA = 4;
  
  procedure CalculateIntegralInPoints(formula:integer; input:tpoints; length:integer; eps:real; var output:tpoints; var actionCount:tpoints);
  procedure CalculateIntegralInPointsByK(formula:integer; input:tpoints; length:integer; K:integer; var output:tpoints);

implementation
  function InferiorLimit:real; //������ ������ ��������������
    begin
     result:=0;
    end;
   
  function Func(x:real):real; //���������� ��������������� ������� � �����
    begin
      result:=2*exp(-sqr(x))/sqrt(pi);
    end;
    
  function CalculateIntegral(t,t1,del:real; formula:integer):real; //���������� ��������� �� t �� t1 �� �������� �������
       //�� ����, del=t1-t
    begin
           if formula=LEFT_RECTANGLES_FROMULA then result:=del*Func(t)
      else if formula=CENTRAL_RECTANGLES_FROMULA then result:=del*Func((t+t1)/2)
      else if formula=TRAPEZOIDS_FROMULA then result:=(Func(t)+Func(t1))*del/2
      else if formula=SIMPSON_FROMULA then result:=(Func(t)+4*Func(t+(del/2))+Func(t1))*del/6
      else if formula=GAUSS_FROMULA then result:=(Func(t+(1-1/sqrt(3))*del/2)+Func(t+(1+1/sqrt(3))*del/2))*del/2;
    end;
    

  function CalculateInPoint(x:real; K:integer; formula:integer):real; //���������� � ����� ����� x � �������� ���������� K (���-�� ���������)
      //=del*f(t)
     var c,del,S,q,t,t1:real; i:integer;
     begin
      c:=InferiorLimit;//c - ������ ������ ���������
      del:= (x-c)/K; //�������� ���������
      t:=c;
      t1:=c+del;
      S:=0;//���������� �����
      for i:=0 to (k-1) do
        begin
          q:=CalculateIntegral(t,t1,del,formula); //��������� � �����
          S:=S+q;
          t:=t1;
          t1:=t1+del;
        end;
       result:=S;
  end;
    
  procedure CalculateIntegralInPoints(formula:integer; input:tpoints; length:integer; eps:real; var output:tpoints; var actionCount:tpoints);
   // ���������� ��������� � ������� ����� � ��������� eps. ���������� actionCount - "����� ��������" � ������ �����
    var  i:integer; x,Sk,S2k:real; k:integer; epsIsReached:boolean;
    begin
        for i:=0 to (length-1) do
          begin
            x:=input[i];
            if (x=InferiorLimit) then
               begin
                 output[i]:=0;
                 actionCount[i]:=0;
               end
                                 else
               begin
                 k:=2;
                 Sk:=CalculateInPoint(x, k, formula); //Sk, S2k - ����������� ��������� � ������� k
                 k:=k*2;
                 S2k:=CalculateInPoint(x, k, formula);
                 epsIsReached:=(abs(Sk-S2k)<eps); //���������� �� �������� (=|Sk-S2k|)
                 while not epsIsReached do
                   begin
                    Sk:=S2k;
                    k:=k*2;
                    S2k:=CalculateInPoint(x, k, formula);
                    epsIsReached:=(abs(Sk-S2k)<eps);
                   end;
                 output[i]:=S2k;
                 actionCount[i]:=k;
                end;
          end;
    end;
    
  procedure CalculateIntegralInPointsByK(formula:integer; input:tpoints; length:integer; K:integer; var output:tpoints);
   // ���������� ��������� � ������� ����� � �������� K. ���������� actionCount - "����� ��������" � ������ �����
    var  i:integer; x,Sk,S2k:real; epsIsReached:boolean;
    begin
        for i:=0 to (length-1) do
          begin
            x:=input[i];
            if (x=InferiorLimit) then
               begin
                 output[i]:=0;
               end
                                 else
               begin
                 output[i]:=CalculateInPoint(x, K, formula); //Sk, S2k - ����������� ��������� � ������� k
                end;
          end;
    end;
end.