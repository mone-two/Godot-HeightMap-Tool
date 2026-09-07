# Godot-HeightMap-Tool
### 条件和局限
- 在Godot4中，由平面做成的地形可以使用此工具来生成高度地图做碰撞体形状。
- 顶点只在垂直方向移动的平面网格的效果最佳，顶点偏移越大，失真越高。
- 目前，生成的高度地图可能需要在引擎里手动调整，才能尽量贴合。
### 在启用工具之前
- 将从外部导入的.gld文件转为本地，在其刚释放出来属性为网格的子节点上建立脚本，提取网格数据，并输出到文件中。
```
		Node3D sampleMap=GetNode<Node3D>(this.GetPath());
		if(sampleMap is MeshInstance3D instance3D)
		{
			var meshData=instance3D.Mesh.SurfaceGetArrays(0);
			File.WriteAllText("output.txt",meshData.ToString());
		}
```
### 工具使用方法
- 在位置偏上的“输入文件”框里，可以点右侧的三个点，找到刚才提取出来的output.txt。
- 在位置偏下的“输出路径”框里，可以点右侧的三个点，选择高度地图tres文件的存储位置。同一行右侧的“输出文件名”框里，可以指定文件名，若留空，则使用默认文件名。
- 最后点击按钮，等待文件输出即可。
