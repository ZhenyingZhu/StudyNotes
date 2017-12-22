package org.liky.game.frame;

import java.awt.*;
import java.awt.image.*;
import java.awt.event.*;
import java.io.*;

import javax.imageio.ImageIO;
import javax.swing.*;

public class FiveChessFrame extends JFrame implements MouseListener, Runnable {
	/**
	 * 
	 */
	private static final long serialVersionUID = 1L;
	private int width = Toolkit.getDefaultToolkit().getScreenSize().width;
	private int height = Toolkit.getDefaultToolkit().getScreenSize().height;
	private int window_size = 500;
	private BufferedImage bgImage = null;
	int x = 0;
	int y = 0;
	int[][] allChess = new int[19][19];
	boolean isBlack = true;
	boolean canPlay = true;
	String message = "Black First";
	int maxTime = 0;
	Thread t = new Thread(this); // Time counter
	int blackTime = 0;
	int whiteTime = 0;
	String blackTimeMessage = "No limit";
	String whiteTimeMessage = "No limit";

	@SuppressWarnings("deprecation")
	public FiveChessFrame() {
		this.setTitle("Five Chess Game");
		this.setSize(window_size, window_size);
		this.setLocation((width - window_size) / 2, (height - window_size) / 2);
		this.setResizable(false);
		this.setDefaultCloseOperation(JFrame.EXIT_ON_CLOSE);
		this.addMouseListener(this);
		this.setVisible(true);

		t.start();
		t.suspend();

		String imagePath = null;
		try {
			imagePath = System.getProperty("user.dir") + "/bin/image/background.jpg";
			bgImage = ImageIO.read(new File(imagePath.replaceAll("\\\\", "/")));
		} catch (IOException e) {
			e.printStackTrace();
		}
		this.repaint();
	}

	public void paint(Graphics g) {
		// BiBuffer
		BufferedImage bi = new BufferedImage(window_size, window_size, BufferedImage.TYPE_INT_RGB);
		Graphics g2 = bi.createGraphics();
		g2.setColor(Color.BLACK);
		// Background
		g2.drawImage(bgImage, 1, 20, this);
		g2.setFont(new Font("Times New Roman", Font.BOLD, 20));
		g2.drawString("Info: " + message, 130, 60);
		g2.setFont(new Font("Calibri", 0, 14));
		g2.drawString("Black Time: " + blackTimeMessage, 30, 470);
		g2.drawString("White Time: " + whiteTimeMessage, 260, 470);
		// Board
		for (int i = 0; i < 19; i++) {
			g2.drawLine(10, 70 + 20 * i, 370, 70 + 20 * i);
			g2.drawLine(10 + 20 * i, 70, 10 + 20 * i, 430);
		}
		// Dot position
		g2.fillOval(68, 128, 4, 4);
		g2.fillOval(308, 128, 4, 4);
		g2.fillOval(308, 368, 4, 4);
		g2.fillOval(68, 368, 4, 4);
		g2.fillOval(308, 248, 4, 4);
		g2.fillOval(188, 128, 4, 4);
		g2.fillOval(68, 248, 4, 4);
		g2.fillOval(188, 368, 4, 4);
		g2.fillOval(188, 248, 4, 4);

		// Draw all white and black Chess
		for (int i = 0; i < 19; i++) {
			for (int j = 0; j < 19; j++) {
				if (allChess[i][j] == 1) {
					// Black
					int tempX = i * 20 + 10;
					int tempY = j * 20 + 70;
					g2.fillOval(tempX - 7, tempY - 7, 14, 14);
				}
				if (allChess[i][j] == 2) {
					// White
					int tempX = i * 20 + 10;
					int tempY = j * 20 + 70;
					g2.setColor(Color.WHITE);
					g2.fillOval(tempX - 7, tempY - 7, 14, 14);
					g2.setColor(Color.BLACK);
					g2.drawOval(tempX - 7, tempY - 7, 14, 14);
				}
			}
		}
		g.drawImage(bi, 0, 0, this);

	}

	@SuppressWarnings("deprecation")
	public void mousePressed(MouseEvent mouse) {
		if (canPlay == true) {
			x = mouse.getX();
			y = mouse.getY();
			if (x >= 10 && x <= 370 && y >= 70 && y <= 430) {
				x = (x - 10) / 20;
				y = (y - 70) / 20;
				if (allChess[x][y] == 0) {
					// Which color
					if (isBlack == true) {
						allChess[x][y] = 1;
						isBlack = false;
						message = "White turn";
					} else {
						allChess[x][y] = 2;
						isBlack = true;
						message = "Black turn";
					}
					// Judge if win
					boolean winFlag = this.checkWin();
					if (winFlag == true) {
						JOptionPane.showMessageDialog(this, "游戏结束," + (allChess[x][y] == 1 ? "Black" : "White") + " Win!");
						canPlay = false;
					}
				} else {
					JOptionPane.showMessageDialog(this, "This position is unavailable");
				}
				this.repaint();
			}
		}
		// Start game
		if (mouse.getX() >= 400 && mouse.getX() <= 470 && mouse.getY() >= 70 && mouse.getY() <= 100) {
			int result = JOptionPane.showConfirmDialog(this, "Ready to start game?");
			if (result == 0) {
				// Restart game
				// 1) Clear the board
				// 2) Reset the clock
				// 3) The first player is black
				for (int i = 0; i < 19; i++) {
					for (int j = 0; j < 19; j++) {
						allChess[i][j] = 0;
					}
				}
				message = "Black First";
				isBlack = true;
				blackTime = maxTime;
				whiteTime = maxTime;
				if (maxTime > 0) {
					blackTimeMessage = maxTime / 3600 + ":" + (maxTime / 60 - maxTime / 3600 * 60) + ":" + (maxTime - maxTime / 60 * 60);
					whiteTimeMessage = maxTime / 3600 + ":" + (maxTime / 60 - maxTime / 3600 * 60) + ":" + (maxTime - maxTime / 60 * 60);
					t.resume();
				} else {
					blackTimeMessage = "Unlimited";
					whiteTimeMessage = "Unlimited";
				}
				this.canPlay = true;
				this.repaint();

			}
		}
		// Setting
		if (mouse.getX() >= 400 && mouse.getX() <= 470 && mouse.getY() >= 120 && mouse.getY() <= 150) {
			String input = JOptionPane.showInputDialog("Please the max time(Miniute), if 0 means unlimited: ");
			try {
				maxTime = Integer.parseInt(input) * 60;
				if (maxTime < 0) {
					JOptionPane.showMessageDialog(this, "Please input valid number! ");
				}
				if (maxTime == 0) {
					int result = JOptionPane.showConfirmDialog(this, "Setting complete, start game?");
					if (result == 0) {
						for (int i = 0; i < 19; i++) {
							for (int j = 0; j < 19; j++) {
								allChess[i][j] = 0;
							}
						}
						message = "Black First";
						isBlack = true;
						blackTime = maxTime;
						whiteTime = maxTime;
						blackTimeMessage = "Unlimited";
						whiteTimeMessage = "Unlimited";
						this.canPlay = true;
						this.repaint();
					}
				}
				if (maxTime > 0) {
					int result = JOptionPane.showConfirmDialog(this, "Setting complete, start game?");
					if (result == 0) {
						for (int i = 0; i < 19; i++) {
							for (int j = 0; j < 19; j++) {
								allChess[i][j] = 0;
							}
						}
						message = "Black First";
						isBlack = true;
						blackTime = maxTime;
						whiteTime = maxTime;
						blackTimeMessage = maxTime / 3600 + ":" + (maxTime / 60 - maxTime / 3600 * 60) + ":" + (maxTime - maxTime / 60 * 60);
						whiteTimeMessage = maxTime / 3600 + ":" + (maxTime / 60 - maxTime / 3600 * 60) + ":" + (maxTime - maxTime / 60 * 60);
						t.resume();
						this.canPlay = true;
						this.repaint();
					}
				}
			} catch (NumberFormatException e1) {
				JOptionPane.showMessageDialog(this, "Please input valid number! ");
			}
		}
		// Info
		if (mouse.getX() >= 400 && mouse.getX() <= 470 && mouse.getY() >= 170 && mouse.getY() <= 200) {
			JOptionPane.showMessageDialog(this, "This is a five chess game");
		}
		// Give up
		if (mouse.getX() >= 400 && mouse.getX() <= 470 && mouse.getY() >= 270 && mouse.getY() <= 300) {
			int result = JOptionPane.showConfirmDialog(this, "Give up");
			if (result == 0) {
				if (isBlack) {
					JOptionPane.showMessageDialog(this, "Black is give up! ");
				} else {
					JOptionPane.showMessageDialog(this, "White is give up! ");
				}
				canPlay = false;
			}
		}
		// About
		if (mouse.getX() >= 400 && mouse.getX() <= 470 && mouse.getY() >= 320 && mouse.getY() <= 350) {
			JOptionPane.showMessageDialog(this, "本游戏由MLDN制作，有相关问题可以访问www.mldn.cn");
		}
		// Exit
		if (mouse.getX() >= 400 && mouse.getX() <= 470 && mouse.getY() >= 370 && mouse.getY() <= 400) {
			JOptionPane.showMessageDialog(this, "Game end");
			System.exit(0);
		}

	}

	private boolean checkWin() {
		boolean flag = false;
		// Count connected number
		int count = 1;
		int color = allChess[x][y];
		// Row
		count = this.checkCount(1, 0, color);
		if (count >= 5) {
			flag = true;
		} else {
			// Column
			count = this.checkCount(0, 1, color);
			if (count >= 5) {
				flag = true;
			} else {
				// Diagonal left down to right up
				count = this.checkCount(1, -1, color);
				if (count >= 5) {
					flag = true;
				} else {
					// Diagonal left up to right down
					count = this.checkCount(1, 1, color);
					if (count >= 5) {
						flag = true;
					}
				}
			}
		}

		return flag;
	}

	// Check connected number
	private int checkCount(int xChange, int yChange, int color) {
		int count = 1;
		int tempX = xChange;
		int tempY = yChange;
		while (x + xChange >= 0 && x + xChange <= 18 && y + yChange >= 0 && y + yChange <= 18 && color == allChess[x + xChange][y + yChange]) {
			count++;
			if (xChange != 0)
				xChange++;
			if (yChange != 0) {
				if (yChange > 0)
					yChange++;
				else {
					yChange--;
				}
			}
		}
		xChange = tempX;
		yChange = tempY;
		while (x - xChange >= 0 && x - xChange <= 18 && y - yChange >= 0 && y - yChange <= 18 && color == allChess[x - xChange][y - yChange]) {
			count++;
			if (xChange != 0)
				xChange++;
			if (yChange != 0) {
				if (yChange > 0)
					yChange++;
				else {
					yChange--;
				}
			}
		}
		return count;
	}

	public void showDia() {
		int result = JOptionPane.showConfirmDialog(this, "Hello", "Choose", JOptionPane.PLAIN_MESSAGE);
		if (result == 0) {
			String input = JOptionPane.showInputDialog(this, "Name");
			JOptionPane.showMessageDialog(this, input);
		}
		if (result == 1) {
			JOptionPane.showMessageDialog(this, "message");
		}
		if (result == 2) {
			JOptionPane.showMessageDialog(this, "Option");
		}
	}

	@Override
	public void run() {
		// Judge max time
		if (maxTime > 0) {
			while (true) {
				if (isBlack) {
					blackTime--;
					if (blackTime == 0) {
						JOptionPane.showMessageDialog(this, "Black run out time! ");
					}
				} else {
					whiteTime--;
					if (whiteTime == 0) {
						JOptionPane.showMessageDialog(this, "White run out time! ");
					}
				}
				blackTimeMessage = blackTime / 3600 + ":" + (blackTime / 60 - blackTime / 3600 * 60) + ":" + (blackTime - blackTime / 60 * 60);
				whiteTimeMessage = whiteTime / 3600 + ":" + (whiteTime / 60 - whiteTime / 3600 * 60) + ":" + (whiteTime - whiteTime / 60 * 60);
				this.repaint();
				try {
					Thread.sleep(1000);
				} catch (InterruptedException e) {
					e.printStackTrace();
				}
				System.out.println(blackTime + " -- " + whiteTime);
			}
		}
	}

	@Override
	public void mouseClicked(MouseEvent arg0) {
		// TODO Auto-generated method stub
	}

	@Override
	public void mouseEntered(MouseEvent arg0) {
		// TODO Auto-generated method stub
	}

	@Override
	public void mouseExited(MouseEvent arg0) {
		// TODO Auto-generated method stub
	}

	@Override
	public void mouseReleased(MouseEvent e) {
		// TODO Auto-generated method stub
	}
}
